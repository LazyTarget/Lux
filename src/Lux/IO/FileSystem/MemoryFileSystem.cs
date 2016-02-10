using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Lux.Diagnostics;

namespace Lux.IO
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly SortedDictionary<string, DirectoryMock> _directories;
        private ILog _log;

        public MemoryFileSystem()
        {
            DefaultEncoding = Encoding.UTF8;
            _directories = new SortedDictionary<string, DirectoryMock>(new PathComparer());
            Log = Framework.LogFactory.GetLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public MemoryFileSystem(ILog log)
            : this()
        {
            Log = log;
        }


        /// <summary>
        /// Collection of directories (Key is directory name)
        /// </summary>
        public IReadOnlyDictionary<string, DirectoryMock> Directories
        {
            get { return new ReadOnlyDictionary<string, DirectoryMock>(_directories); }
        }
        
        public Encoding DefaultEncoding { get; set; }

        public ILog Log
        {
            get { return _log; }
            set
            {
                if (value == null)
                    _log = new NullObjectLog();
                else
                    _log = value;
            }
        }
        

        public FileMock GetFile(string path)
        {
            var file = GetFileOrDefault(path);
            if (file == null)
            {
                _log.Debug($"GetFile() Path: {path}; Error: {Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND}");
                throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND, path);
            }
            return file;
        }
        
        public FileMock GetFileOrDefault(string path)
        {
            var dirPath = PathHelper.GetParentOrDefault(path);
            if (string.IsNullOrEmpty(dirPath))
            {
                //throw new DirectoryNotFoundException(Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
                return null;
            }
            var dir = GetDirectory(dirPath);
            if (dir == null)
            {
                //throw new DirectoryNotFoundException(Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
                return null;
            }
            var fileName = Path.GetFileName(path);
            var file = dir.Files.ContainsKey(fileName) ? dir.Files[fileName] : null;
            return file;
        }
        

        public void AddFile(FileMock file)
        {
            var dirPath = PathHelper.GetParentOrDefault(file.Path);
            if (string.IsNullOrEmpty(dirPath))
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
            var dir = GetDirectory(dirPath);
            if (dir == null)
            {
                dir = CreateDirectory(dirPath);
                dir = GetDirectory(dirPath);
            }
            dir.AddFile(file);
        }



        public IEnumerable<string> EnumerateDrives()
        {
            var rootDirectories = _directories.Select(x => x.Value);
            var drives = rootDirectories.Select(x => x.Path);
            return drives;
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            var directory = GetDirectory(path);
            if (directory != null)
            {
                _log.Debug($"EnumerateDirectories() Path: {path}, SearchPattern: {searchPattern}, SearchOption: {searchOption}");
                var directories = directory.EnumerateDirectories(searchPattern, searchOption);
                var paths = directories.Select(x => x.Path);
                return paths;
            }
            else
            {
                _log.Debug($"EnumerateDirectories() Path: {path}, SearchPattern: {searchPattern}, SearchOption: {searchOption}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
            }
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var directory = GetDirectory(path);
            if (directory != null)
            {
                _log.Debug($"EnumerateFiles() Path: {path}, SearchPattern: {searchPattern}, SearchOption: {searchOption}");
                var files = directory.EnumerateFiles(searchPattern, searchOption);
                var paths = files.Select(x => x.Path);
                return paths;
            }
            else
            {
                _log.Debug($"EnumerateFiles() Path: {path}, SearchPattern: {searchPattern}, SearchOption: {searchOption}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
            }
        }



        public bool FileExists(string path)
        {
            var file = GetFileOrDefault(path);
            var exists = file != null;
            var isDeleted = exists && file.IsDeleted;
            var res = exists && !isDeleted;
            _log.Debug($"Exists() Path: {path}; Result: {res}, IsDeleted: {isDeleted}");
            return res;
        }

        public void MoveFile(string sourceFileName, string destFileName)
        {
            if (PathHelper.AreEquivalent(sourceFileName, destFileName))
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE}");
                throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE);
            }

            var file = GetFileOrDefault(sourceFileName);
            if (file == null)
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_FILE_NOT_FOUND}");
                throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_FILE_NOT_FOUND, sourceFileName);
            }

            var targetFile = GetFileOrDefault(destFileName);
            if (targetFile != null)
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_FILE_ALREADY_EXISTS}");
                throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_FILE_ALREADY_EXISTS);
            }

            var sourceDirPath = PathHelper.GetParent(sourceFileName);
            var sourceDir = GetDirectory(sourceDirPath);
            if (sourceDir == null)
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND);
            }

            var targetDirPath = PathHelper.GetParent(destFileName);
            var targetDir = GetDirectory(targetDirPath);
            if (targetDir == null)
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND);
            }

            sourceDir.MoveFile(file, destFileName, targetDir);
            _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}");
        }

        public void CopyFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (PathHelper.AreEquivalent(sourceFileName, destFileName))
            {
                _log.Debug($"MoveFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE}");
                throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE);
            }

            var file = GetFileOrDefault(sourceFileName);
            if (file == null)
            {
                _log.Debug($"CopyFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_FILE_NOT_FOUND}");
                throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_FILE_NOT_FOUND, sourceFileName);
            }

            if (!overwrite)
            {
                var targetFile = GetFileOrDefault(destFileName);
                if (targetFile != null)
                {
                    _log.Debug($"CopyFile() Source: {sourceFileName}, Target: {destFileName}; " +
                               $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_FILE_ALREADY_EXISTS}");
                    throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_FILE_ALREADY_EXISTS);
                }
            }

            var sourceDirPath = PathHelper.GetParent(sourceFileName);
            var sourceDir = GetDirectory(sourceDirPath);
            if (sourceDir == null)
            {
                _log.Debug($"CopyFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND);
            }

            var targetDirPath = PathHelper.GetParent(destFileName);
            var targetDir = GetDirectory(targetDirPath);
            if (targetDir == null)
            {
                _log.Debug($"CopyFile() Source: {sourceFileName}, Target: {destFileName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND);
            }

            var newFile = FileMock.Copy(file, destFileName);
            targetDir.AddFile(newFile);
            _log.Debug($"CopyFile() Source: {sourceFileName}, Target: {destFileName}");
        }

        public void DeleteFile(string path)
        {
            var file = GetFileOrDefault(path);
            if (file != null)
            {
                var dirPath = PathHelper.GetParent(file.Path);
                var dir = GetDirectory(dirPath);
                if (dir != null)
                {
                    _log.Debug($"DeleteFile() Path: {path}");
                    dir.DeleteFile(file);
                }
                else
                {
                    _log.Debug($"DeleteFile() Path: {path}; Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                    throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
                }
            }
            else
            {
                _log.Debug($"DeleteFile() Path: {path}; Error: {Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND}");
                throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND, path);
            }
        }


        public Stream OpenFile(string fileName, FileMode mode, FileAccess access)
        {
            return OpenFile(fileName, mode, access, FileShare.None);
        }

        public Stream OpenFile(string fileName, FileMode mode, FileAccess access, FileShare share)
        {
            bool fileCreated = false;
            var file = GetFileOrDefault(fileName);
            if (file == null)
            {
                if (mode != FileMode.CreateNew && mode != FileMode.OpenOrCreate)
                {
                    _log.Debug($"Open() FileName: {fileName}, FileMode: {mode}, FileAccess: {access}; " +
                               $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND}");
                    throw new FileNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND, fileName);
                }
                file = FileMock.Create(fileName);
                var dirPath = PathHelper.GetParent(fileName);
                var dir = GetDirectory(dirPath);
                if (dir == null)
                {
                    _log.Debug($"Open() FileName: {fileName}, FileMode: {mode}, FileAccess: {access}; " +
                               $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                    throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
                }
                file.Encoding = DefaultEncoding;
                fileCreated = true;
                dir.AddFile(file);
            }
            var stream = file.GetStream();
            if (stream.CanSeek)
            {
                if (mode == FileMode.Append)
                    stream.Position = stream.Length;
                else
                    stream.Position = 0;
            }
            _log.Debug($"Open() FileName: {fileName}, FileMode: {mode}, FileAccess: {access}; " +
                       $"Stream length: {stream.Length}, Stream position: {stream.Position}, Created: {fileCreated}");
            return stream;
        }


        public DirectoryMock GetDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            //var res = _directories.ContainsKey(path) ? _directories[path] : null;
            DirectoryMock res = null;
            var comparer = new SubPathComparer();

            var directories = _directories.Select(x => x.Value).ToList();
            do
            {
                var newDirectories = false;
                foreach (var dir in directories)
                {
                    //var c = comparer.Compare(dir.Path, path);
                    var c = comparer.Compare(path, dir.Path);
                    if (c == 0)
                    {
                        res = dir;
                        break;
                    }
                    if (c > 0)
                    {
                        newDirectories = true;
                        directories = dir.Directories.Select(x => x.Value).ToList();
                        break;
                    }
                }
                if (!newDirectories)
                    break;
            } while (res == null);

            return res;
        }

        public DirectoryMock GetClosestDirectory(string path)
        {
            var res = GetDirectory(path);
            var p = path;
            while (res == null && !string.IsNullOrEmpty(p))
            {
                p = PathHelper.GetParentOrDefault(p);
                if (string.IsNullOrEmpty(p))
                {
                    res = null;
                    break;
                }
                res = GetDirectory(p);
            }
            return res;
        }


        
        public bool DirExists(string path)
        {
            var dir = GetDirectory(path);
            var exists = dir != null;
            var isDeleted = exists && dir.IsDeleted;
            var res = exists && !isDeleted;
            _log.Debug($"DirExists() Path: {path}; Result: {res}, IsDeleted: {isDeleted}");
            return res;
        }
        

        public DirectoryMock CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var closest = GetClosestDirectory(path);
            if (closest == null)
            {
                var rootPath = PathHelper.GetRootParent(path);
                closest = DirectoryMock.Create(rootPath);
                _directories.Add(rootPath, closest);
            }
            else if (PathHelper.AreEquivalent(closest.Path, path))
            {
                //Debug(string.Format("CreateDirectory() Path: {0}; Error: {1}", path, Consts.ErrorMessages.ERROR_TARGET_DIR_ALREADY_EXISTS));
                //throw new IOException(Consts.ErrorMessages.ERROR_TARGET_DIR_ALREADY_EXISTS);
            }

            var dir = closest;
            while (!PathHelper.AreEquivalent(dir.Path, path))
            {
                var relPath = PathHelper.Subtract(path, dir.Path);
                var parts = PathHelper.GetPathParts(relPath);
                var newDirName = parts.First();
                var newDirPath = PathHelper.Combine(dir.Path, newDirName);
                var newDir = DirectoryMock.Create(newDirPath);
                dir.AddDirectory(newDir);
                dir = newDir;
            }
            var res = PathHelper.AreEquivalent(dir.Path, path)
                          ? dir
                          : null;
            _log.Debug($"CreateDirectory() Path: {path}; Result: {res != null}");
            if (res == null)
                throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_CREATING_DIR);
            return res;
        }

        public void CreateDir(string path)
        {
            var dir = CreateDirectory(path);
        }

        public void MoveDir(string sourceDirName, string destDirName)
        {
            if (PathHelper.AreEquivalent(sourceDirName, destDirName))
            {
                _log.Debug($"MoveFile() Source: {sourceDirName}, Target: {destDirName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE}");
                throw new IOException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_SAME_AS_SOURCE);
            }

            var dir = GetDirectory(sourceDirName);
            if (dir == null)
            {
                _log.Debug($"MoveDirectory() Source: {sourceDirName}, Target: {destDirName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_SOURCE_DIR_NOT_FOUND);
            }
            var targetDir = GetDirectory(destDirName);
            if (targetDir == null)
            {
                _log.Debug($"MoveDirectory() Source: {sourceDirName}, Target: {destDirName}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_TARGET_DIR_NOT_FOUND);
            }
            _log.Debug($"MoveDirectory() Source: {sourceDirName}, Target: {destDirName}");
            MoveDirectory(dir, targetDir);
        }
        
        private void MoveDirectory(DirectoryMock directory, DirectoryMock target)
        {
            var parentDirPath = PathHelper.GetParent(directory.Path);
            var parentDir = GetDirectory(parentDirPath);
            if (parentDir == null)
            {
                _log.Debug($"MoveDirectory() Source: {directory}, Target: {target}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
            }
            parentDir.MoveSubDirectory(directory, target);


            var files = directory.Files.ToList();
            var directories = directory.Directories.SelectMany(x => x.Value.Directories).ToList();

            foreach (var file in files.Select(x => x.Value))
            {
                var targetRelPath = PathHelper.Subtract(file.Path, directory.Path);
                var targetPath = PathHelper.Combine(target.Path, targetRelPath);
                directory.MoveFile(file, targetPath, target);
            }

            foreach (var dir in directories.Select(x => x.Value))
            {
                var targetRelPath = PathHelper.Subtract(dir.Path, directory.Path);
                var targetPath = PathHelper.Combine(target.Path, targetRelPath);
                var targetDir = CreateDirectory(targetPath);
                MoveDirectory(dir, targetDir);
            }
        }
        
        public void DeleteDir(string path, bool recursive)
        {
            var dir = GetDirectory(path);
            if (dir != null)
            {
                var parentDirPath = PathHelper.GetParent(path);
                var parentDir = GetDirectory(parentDirPath);
                if (parentDir != null)
                {
                    _log.Debug($"DeleteDirectory() Path: {path}, Recursive: {recursive}");
                    parentDir.DeleteDirectory(dir, recursive);
                }
                else
                {
                    _log.Debug($"DeleteDirectory() Path: {path}, Recursive: {recursive}; " +
                               $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                    throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
                }
            }
            else
            {
                _log.Debug($"DeleteDirectory() Path: {path}, Recursive: {recursive}; " +
                           $"Error: {Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND}");
                throw new DirectoryNotFoundException(Lux.IO.Consts.ErrorMessages.ERROR_DIR_NOT_FOUND);
            }
        }

    }
}
