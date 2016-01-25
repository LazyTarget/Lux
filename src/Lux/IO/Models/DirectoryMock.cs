using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Lux.IO
{
    public class DirectoryMock
    {
        private readonly SortedDictionary<string, FileMock> _files;
        private readonly SortedDictionary<string, DirectoryMock> _directories;
        
        private DirectoryMock(string path)
        {
            Path = path;
            //Name = System.IO.Path.GetDirectoryName(path);
            Name = System.IO.Path.GetFileName(path);
            _files = new SortedDictionary<string, FileMock>(new PathComparer());
            _directories = new SortedDictionary<string, DirectoryMock>(new PathComparer());
        }


        public string Name { get; private set; }

        public string Path { get; private set; }

        public bool IsDeleted { get; protected internal set; }

        
        /// <summary>
        /// Collection of sub directories (key is directory name)
        /// </summary>
        public IReadOnlyDictionary<string, DirectoryMock> Directories
        {
            get { return new ReadOnlyDictionary<string, DirectoryMock>(_directories); }
        }

        /// <summary>
        /// Collection of files (key is file name)
        /// </summary>
        public IReadOnlyDictionary<string, FileMock> Files
        {
            get { return new ReadOnlyDictionary<string, FileMock>(_files); }
        }



        public override string ToString()
        {
            return Path;
        }


        protected internal void AddDirectory(DirectoryMock dir)
        {
            _directories.Add(dir.Name, dir);
        }

        protected internal void AddFile(FileMock file)
        {
            _files.Add(file.Name, file);
        }

        protected internal void MoveFile(FileMock file, string targetPath, DirectoryMock targetDir)
        {
            var r = _files.Remove(file.Name);
            file.Path = targetPath;
            file.Name = System.IO.Path.GetFileName(file.Path);
            targetDir._files.Add(file.Name, file);
        }

        protected internal void MoveSubDirectory(DirectoryMock directory, DirectoryMock targetDir)
        {
            var r = _directories.Remove(directory.Name);
            //directory.Path = targetPath;
            //directory.Name = System.IO.Path.GetDirectoryName(targetPath);
            targetDir._directories.Add(directory.Name, directory);
        }

        protected internal void DeleteFile(FileMock file)
        {
            file.IsDeleted = true;
            var r = _files.Remove(file.Name);
        }

        protected internal void DeleteDirectory(DirectoryMock directory, bool recursive)
        {
            var files = directory.Files.ToList();
            var directories = directory.Directories.SelectMany(x => x.Value.Directories).ToList();
            if (!recursive)
            {
                if (files.Any() || directories.Any())
                    throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_EMPTY);
            }


            foreach (var file in files.Select(x => x.Value))
            {
                directory.DeleteFile(file);
            }

            foreach (var dir in directories.Select(x => x.Value))
            {
                directory.DeleteDirectory(dir, true);
            }

            directory.IsDeleted = true;
            var r = _directories.Remove(directory.Name);
        }


        public IEnumerable<FileMock> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            var result = new List<FileMock>();
            result.AddRange(Files.Where(x => PathHelper.ValidateSearchPattern(x.Value.Name, searchPattern))
                                 .Select(x => x.Value)
                                 .ToList());
            
            if (searchOption == SearchOption.AllDirectories)
            {
                var enumerable = Directories.Select(x => x.Value).SelectMany(x => x.EnumerateFiles(searchPattern, searchOption));
                result.AddRange(enumerable);
            }
            return result;
        }


        public IEnumerable<DirectoryMock> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            // todo: implement searchPattern

            var result = new List<DirectoryMock>();
            result.AddRange(Directories.Where(x => PathHelper.ValidateSearchPattern(x.Value.Name, searchPattern))
                                       .Select(x => x.Value)
                                       .ToList());

            if (searchOption == SearchOption.AllDirectories)
            {
                var enumerable = Directories.Select(x => x.Value).SelectMany(x => x.EnumerateDirectories(searchPattern, searchOption));
                result.AddRange(enumerable);
            }
            return result;
        }
 


        public static DirectoryMock Create(string path)
        {
            var file = new DirectoryMock(path);
            return file;
        }

        public static DirectoryMock Copy(DirectoryMock directory, string targetPath)
        {
            var newDir = Create(targetPath);
            foreach (var dir in directory.Directories.Select(x => x.Value))
            {
                var newSubDirPath = PathHelper.Combine(targetPath, dir.Name);
                var newSubDir = Copy(dir, newSubDirPath);
                newDir._directories.Add(newSubDir.Name, newSubDir);
            }
            foreach (var file in directory.Files.Select(x => x.Value))
            {
                var newFilePath = PathHelper.Combine(directory.Path, file.Name);
                var newFile = FileMock.Copy(file, newFilePath);
                newDir._files.Add(newFile.Name, newFile);
            }
            return newDir;
        }

    }
}
