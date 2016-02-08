using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Lux.Extensions;

namespace Lux.IO
{
    public class FileSystemHelper
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }



        public SortedDictionary<string, DirectoryMock> GetStructure(string path, bool loadDirectories = true, bool loadFiles = true, bool recursive = true)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (!_fileSystem.DirExists(path))
                return null;

            var result = new SortedDictionary<string, DirectoryMock>(new PathComparer());
            var directory = DirectoryMock.Create(path);
            result.Add(path, directory);

            if (loadFiles)
            {
                try
                {
                    var enumerable = _fileSystem.EnumerateFiles(path, null, SearchOption.TopDirectoryOnly);
                    foreach (var filePath in enumerable)
                    {
                        try
                        {
                            var file = FileMock.Create(filePath);
                            directory.AddFile(file);
                        }
                        catch (Exception ex)
                        {
                            //throw;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (loadDirectories)
            {
                try
                {
                    var enumerable = _fileSystem.EnumerateDirectories(path, null, SearchOption.TopDirectoryOnly);
                    foreach (var dirPath in enumerable)
                    {
                        try
                        {
                            var structure = GetStructure(dirPath, recursive, loadFiles);
                            string path1 = dirPath;
                            var dir = structure.Select(x => x.Value)
                                               .Single()
                                               .Directories.Where(x => x.Key == path1)
                                               .Select(x => x.Value)
                                               .SingleOrDefault();
                            if (dir != null)
                                directory.AddDirectory(dir);
                        }
                        catch (Exception ex)
                        {
                            //throw;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return result;
        }



        public FileMock GetFile(string path)
        {
            var file = GetFileOrDefault(path);
            if (file == null)
                throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND, path);
            return file;
        }
        
        public FileMock GetFileOrDefault(string path)
        {
            FileMock file;
            if (_fileSystem is MemoryFileSystem)
            {
                var memoryFileStream = (MemoryFileSystem)_fileSystem;
                file = memoryFileStream.GetFileOrDefault(path);
                return file;
            }

            if (!_fileSystem.FileExists(path))
                return null;
            
            var content = LazyOpen(path, FileMode.Open, FileAccess.Read);
            file = FileMock.Create(path, content);
            return file;
        }

        public DirectoryMock GetDirectory(string path)
        {
            DirectoryMock dir;
            if (_fileSystem is MemoryFileSystem)
            {
                var memoryFileStream = (MemoryFileSystem)_fileSystem;
                dir = memoryFileStream.GetDirectory(path);
                return dir;
            }

            var structure = GetStructure(path, true, true, true);
            dir = structure.Select(x => x.Value).Single();
            return dir;
        }


        public void AddFile(FileMock file)
        {
            if (_fileSystem is MemoryFileSystem)
            {
                var memoryFileStream = (MemoryFileSystem)_fileSystem;
                memoryFileStream.AddFile(file);
                return;
            }
            
            var dirPath = PathHelper.GetParent(file.Path);
            _fileSystem.CreateDir(dirPath);
            using (var stream = _fileSystem.OpenFile(file.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var s = file.GetStream())
                {
                    s.CopyTo(stream);
                }
                stream.Flush();
            }
        }


        public IEnumerable<FileMock> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var paths = _fileSystem.EnumerateFiles(path, searchPattern, searchOption);
            foreach (var p in paths)
            {
                var bytes = LazyOpen(p, FileMode.Open, FileAccess.Read);
                var file = FileMock.Create(p, bytes);
                yield return file;
            }
        }

        public IEnumerable<FileMock> EnumerateFiles(string path, string searchPattern, SearchOption searchOption, FileShare fileShare)
        {
            var paths = _fileSystem.EnumerateFiles(path, searchPattern, searchOption);
            foreach (var p in paths)
            {
                var bytes = LazyOpen(p, FileMode.Open, FileAccess.Read, fileShare);
                var file = FileMock.Create(p, bytes);
                yield return file;
            }
        }


        public void CopyDir(string sourceDirName, string targetDirName, bool overwrite)
        {
            if (!_fileSystem.DirExists(targetDirName))
                _fileSystem.CreateDir(targetDirName);

            var files = _fileSystem.EnumerateFiles(sourceDirName, null, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var targetFilePath = PathHelper.Combine(targetDirName, fileName);
                _fileSystem.CopyFile(file, targetFilePath, overwrite);
            }

            var directories = _fileSystem.EnumerateDirectories(sourceDirName, null, SearchOption.TopDirectoryOnly);
            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName(directory);
                var targetDirPath = PathHelper.Combine(targetDirName, dirName);
                CopyDir(directory, targetDirPath, overwrite);
            }
        }


        /// <summary>
        /// Find files with wildcard support. Must end with filename pattern (ex. *.log, *.exe|*.dll). Example pattern: "C:/SolutionFolder/ProjectFolder/bin/*/**/*.*"
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> FindFilesWildcard(string pattern)
        {
            var result = new List<string>();
            var parts = PathHelper.GetPathParts(pattern);
            var filenamePattern = parts.Length > 0 ? parts[parts.Length - 1] : null;        // The file pattern must be specified last and is required
            var filenamePatterns = new[] { filenamePattern };
            if (!string.IsNullOrEmpty(filenamePattern) && filenamePattern.Contains('|'))
            {
                filenamePatterns = filenamePattern.Split('|').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (filenamePatterns.Length > 1)
                    filenamePattern = "*";
            }
            Func<string, bool> filePatternFilter = delegate(string path)
            {
                if (filenamePatterns != null && filenamePatterns.Any())
                {
                    var name = Path.GetFileName(path);
                    var valid = filenamePatterns.Any(ptrn => PathHelper.ValidateSearchPattern(name, ptrn));
                    return valid;
                }
                return true;
            };
            

            if (pattern.IndexOf('*') < 0)
            {
                if (filenamePattern != null && filenamePattern.Contains('.'))
                    result = new List<string> { pattern };       // the pattern is a file
                else
                    result = _fileSystem.EnumerateFiles(pattern, filenamePattern, SearchOption.AllDirectories).Where(filePatternFilter).ToList();
            }
            else
            {
                var path = "";
                for (var i = 0; i < parts.Length; i++)
                {
                    var part = parts[i];
                    var isLast = i == (parts.Length - 1);
                    var nextLast = i == (parts.Length - 2);
                    var hasWildcards = part.Contains('*') || part.Contains('?');
                    var dirSearchPattern = part;
                    if (isLast && part != "**")
                    {
                        continue;
                    }
                    if (!hasWildcards)
                    {
                        path = PathHelper.Combine(path, part);
                        if (!nextLast)
                            continue;
                    }

                    
                    var searchOption = part != "*" && ( part == "**" || nextLast )
                                            ? SearchOption.AllDirectories
                                            : SearchOption.TopDirectoryOnly;

                    //if (searchOption == SearchOption.AllDirectories)
                    if (!isLast && ( ( !hasWildcards || part == "**" ) ))
                    {
                        var files = _fileSystem.EnumerateFiles(path, filenamePattern, searchOption).Where(filePatternFilter);
                        result.AddRange(files);
                    }


                    if (!nextLast || hasWildcards)
                    {
                        var directories = _fileSystem.EnumerateDirectories(path, dirSearchPattern, searchOption);
                        foreach (var directory in directories)
                        {
                            var files = _fileSystem.EnumerateFiles(directory, filenamePattern, searchOption).Where(filePatternFilter);
                            result.AddRange(files);
                        }
                    }
                    
                }
            }
            var result2 = result.Select(PathHelper.Normalize)
                                .Distinct()
                                .OrderBy(x => x, new PathSorter())
                                .ToList();
            result = result2;
            return result;
        }


        public Lazy<byte[]> LazyOpen(string path, FileMode mode, FileAccess access)
        {
            var bytes = new Lazy<byte[]>(() =>
            {
                using (var stream = _fileSystem.OpenFile(path, mode, access))
                {
                    var value = stream.ReadAllAsBytes();
                    return value;
                }
            });
            return bytes;
        }

        public Lazy<byte[]> LazyOpen(string path, FileMode mode, FileAccess access, FileShare share)
        {
            var bytes = new Lazy<byte[]>(() =>
            {
                using (var stream = _fileSystem.OpenFile(path, mode, access, share))
                {
                    var value = stream.ReadAllAsBytes();
                    return value;
                }
            });
            return bytes;
        }


        public void ArchiveFiles(string archiveFilePath, IList<PathInfo> files)
        {
            if (files == null || !files.Any())
                return;
            using (var archiveFileStream = _fileSystem.OpenFile(archiveFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var archive = new ZipArchive(archiveFileStream, ZipArchiveMode.Update))
            {
                foreach (var pathInfo in files)
                {
                    if (!_fileSystem.FileExists(pathInfo.AbsolutePath))
                    {
                        System.Diagnostics.Trace.WriteLine("File could not be found for package.supkg: \"{0}\"", pathInfo.AbsolutePath);
                        //throw new FileNotFoundException("File could not be found", file.AbsolutePath);
                    }

                    var entryName = pathInfo.RelativePath;
                    var entry = archive.GetEntry(entryName);
                    if (entry == null)
                        entry = archive.CreateEntry(entryName);
                    using (var entryStream = entry.Open())
                    using (var entryFileStream = _fileSystem.OpenFile(pathInfo.AbsolutePath, FileMode.Open, FileAccess.Read))
                    {
                        entryFileStream.CopyTo(entryStream);
                        entryStream.Flush();
                    }
                }
            }
        }

    }
}