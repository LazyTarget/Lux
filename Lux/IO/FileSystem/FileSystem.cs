using System.Collections.Generic;

namespace Lux.IO
{
    public class FileSystem : IFileSystem
    {
        public IEnumerable<string> EnumerateDrives()
        {
            var res = System.IO.Directory.GetLogicalDrives();
            return res;
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            var res = System.IO.Directory.EnumerateDirectories(path, searchPattern ?? "*", searchOption);
            return res;
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, System.IO.SearchOption searchOption)
        {
            var res = System.IO.Directory.EnumerateFiles(path, searchPattern ?? "*", searchOption);
            return res;
        }


        public bool FileExists(string path)
        {
            var res = System.IO.File.Exists(path);
            return res;
        }

        public void MoveFile(string sourceFileName, string destFileName)
        {
            System.IO.File.Move(sourceFileName, destFileName);
        }

        public void CopyFile(string sourceFileName, string destFileName, bool overwrite)
        {
            System.IO.File.Copy(sourceFileName, destFileName, overwrite);
        }

        public void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public System.IO.Stream OpenFile(string path, System.IO.FileMode mode, System.IO.FileAccess access)
        {
            var stream = System.IO.File.Open(path, mode, access);
            return stream;
        }

        public System.IO.Stream OpenFile( string path, System.IO.FileMode mode, System.IO.FileAccess access,
                                          System.IO.FileShare share )
        {
            var stream = System.IO.File.Open( path, mode, access, share );
            return stream;
        }


        public bool DirExists(string path)
        {
            var res = System.IO.Directory.Exists(path);
            return res;
        }

        public void CreateDir(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        public void MoveDir(string sourceDirName, string destDirName)
        {
            System.IO.Directory.Move(sourceDirName, destDirName);
        }

        public void DeleteDir(string path, bool recursive)
        {
            System.IO.Directory.Delete(path, recursive);
        }

    }
}
