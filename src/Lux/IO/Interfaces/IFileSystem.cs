using System.Collections.Generic;
using System.IO;

namespace Lux.IO
{
    public interface IFileSystem
    {
        IEnumerable<string> EnumerateDrives(); 

        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);


        bool FileExists(string path);

        void MoveFile(string sourceFileName, string destFileName);

        void CopyFile(string sourceFileName, string destFileName, bool overwrite);

        void DeleteFile(string path);

        Stream OpenFile(string fileName, FileMode mode, FileAccess access);

        Stream OpenFile(string fileName, FileMode mode, FileAccess access, FileShare share);


        bool DirExists(string path);

        void CreateDir(string path);

        void MoveDir(string sourceDirName, string destDirName);

        void DeleteDir(string path, bool recursive);
    }
}
