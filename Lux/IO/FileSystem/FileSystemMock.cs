using System.IO;
using Moq;

namespace Lux.IO
{
    public class FileSystemMock
    {
        public static FileSystemMock Create()
        {
            var context = new FileSystemMock();
            return context;
        }

        public static FileSystemMock Create(IFileSystem baseFileSystem)
        {
            var context = new FileSystemMock(baseFileSystem);
            return context;
        }
        
        
        private readonly DebugMock<IFileSystem> _mock;
        private readonly IFileSystem _baseFileSystem;
        
        
        public FileSystemMock()
        {
            _baseFileSystem = new MemoryFileSystem();
            
            //_mock = new Mock<IFileSystem>();
            _mock = new DebugMock<IFileSystem>(this);

            Setup();
        }

        public FileSystemMock(IFileSystem baseFileSystem)
        {
            _baseFileSystem = baseFileSystem;

            //_mock = new Mock<IFileSystem>();
            _mock = new DebugMock<IFileSystem>(this);

            Setup();
        }
        

        public DebugMock<IFileSystem> Mock
        {
            get { return _mock; }
        } 

        public IFileSystem Object
        {
            get { return _mock.Object; }
        }

        public IFileSystem BaseFileSystem
        {
            get { return _baseFileSystem; }
        }

        public FileSystemHelper BaseFileSystemHelper
        {
            get { return new FileSystemHelper(_baseFileSystem); }
        }
        

        private void Setup()
        {
            _mock.Setup(x => x.DirExists(It.IsAny<string>()))
                 .Returns<string>(_baseFileSystem.DirExists);

            _mock.Setup(x => x.CreateDir(It.IsAny<string>()))
                 .Callback<string>((path) => _baseFileSystem.CreateDir(path));

            _mock.Setup(x => x.MoveDir(It.IsAny<string>(), It.IsAny<string>()))
                 .Callback<string, string>(_baseFileSystem.MoveDir);
            
            _mock.Setup(x => x.DeleteDir(It.IsAny<string>(), It.IsAny<bool>()))
                 .Callback<string, bool>(_baseFileSystem.DeleteDir);
            
            _mock.Setup(x => x.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                 .Returns<string, string, SearchOption>((path, searchPattern, searchOption) =>
                                                        _baseFileSystem.EnumerateDirectories(path, searchPattern, searchOption));
            
            _mock.Setup(x => x.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                 .Returns<string, string, SearchOption>((path, searchPattern, searchOption) =>
                                                        _baseFileSystem.EnumerateFiles(path, searchPattern, searchOption));

            _mock.Setup(x => x.FileExists(It.IsAny<string>()))
                    .Returns<string>(_baseFileSystem.FileExists);

            _mock.Setup(x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>()))
                    .Callback<string, string>(_baseFileSystem.MoveFile);
            
            _mock.Setup(x => x.CopyFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                 .Callback<string, string, bool>((source, target, overwrite) => _baseFileSystem.CopyFile(source, target, overwrite));

            _mock.Setup(x => x.DeleteFile(It.IsAny<string>()))
                    .Callback<string>(_baseFileSystem.DeleteFile);

            _mock.Setup(x => x.OpenFile(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
                    .Returns<string, FileMode, FileAccess>(_baseFileSystem.OpenFile);

            _mock.Setup(x => x.OpenFile(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>(), It.IsAny<FileShare>()))
                    .Returns<string, FileMode, FileAccess, FileShare>(_baseFileSystem.OpenFile);
        }


    }
}
