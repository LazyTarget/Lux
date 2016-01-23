using System.Linq;
using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
#if INTEGRATION_TESTS
    
#else
    [Ignore("Integration tests disabled")]
#endif
    [TestFixture]
    public class ZipFileCompressorMockWithRealFileSystem_Tests
    {
        [TestCase]
        public void Archive()
        {
            const string archiveFileName        = "archive.zip";
            const string archiveRootFolder      = "C:/io_testing";
            const string archiveOutputPath      = archiveRootFolder + "/" + archiveFileName;
            const string archiveFolder          = archiveRootFolder + "/files";
            const string fileName1              = archiveFolder + "/file1.txt";
            const string fileName2              = archiveFolder + "/file2.txt";
            const string fileName3              = archiveFolder + "/subfolder/file3.txt";

            var files = DataFactory.CreateFiles(fileName1,
                                                fileName2,
                                                fileName3).ToList();
            
            var fileSystem = new FileSystem();
            var directoryManager = new FileSystemHelper(fileSystem);
            if (fileSystem.DirExists(archiveRootFolder))
                fileSystem.DeleteDir(archiveRootFolder, true);
            files.ForEach(directoryManager.AddFile);
            var sut = new ZipFileCompressorMock(fileSystem);

            sut.CreateFromDirectory(archiveFolder, archiveOutputPath);

            Assert.IsTrue(fileSystem.FileExists(archiveOutputPath));
            
            var file = directoryManager.GetFile(archiveOutputPath);
            Assert.AreEqual(archiveFileName, file.Name);
            Assert.AreEqual(archiveOutputPath, file.Path);
            //Assert.IsInstanceOfType(file, typeof(ZipFileMock));
            //var zipFile = (ZipFileMock)file;
            //Assert.AreEqual(files.Count, zipFile.FileCount);
        }



        [TestCase]
        public void ArchiveAndUnarchive()
        {
            const string archiveFileName        = "archive.zip";
            const string archiveRootFolder      = "C:/io_testing";
            const string archiveOutputPath      = archiveRootFolder + "/" + archiveFileName;
            const string archiveFolder          = archiveRootFolder + "/files";
            const string fileName1              = archiveFolder + "/file1.txt";
            const string fileName2              = archiveFolder + "/file2.txt";
            const string fileName3              = archiveFolder + "/subfolder/file3.txt";

            var files = DataFactory.CreateFiles(fileName1,
                                                fileName2,
                                                fileName3).ToList();

            var fileSystem = new FileSystem();
            var directoryManager = new FileSystemHelper(fileSystem);
            if (fileSystem.DirExists(archiveRootFolder))
                fileSystem.DeleteDir(archiveRootFolder, true);
            files.ForEach(directoryManager.AddFile);
            var sut = new ZipFileCompressorMock(fileSystem);

            var zipFile = sut.CreateFromDirectory(archiveFolder, archiveOutputPath);
            Assert.IsTrue(fileSystem.FileExists(archiveOutputPath));

            //var file = directoryManager.GetFile(archiveOutputPath);
            //Assert.AreEqual(archiveFileName, file.Name);
            //Assert.AreEqual(archiveOutputPath, file.Path);
            //Assert.IsInstanceOfType(file, typeof(ZipFileMock));
            //var zipFile = (ZipFileMock)file;

            Assert.AreEqual(files.Count, zipFile.FileCount);


            const string unarchiveOutputFolder = archiveRootFolder + "/output";

            var i = 0;
            //var extractedFiles = ZipFileMock.Unarchive(file.Bytes, unarchiveOutputFolder);
            var extractedFiles = sut.ExtractToDirectory(archiveOutputPath, unarchiveOutputFolder);
            foreach (var extractedFile in extractedFiles)
            {
                var f = files[i++];
                var relativePath = PathHelper.Subtract(f.Path, archiveFolder);
                var expectedPath = PathHelper.Combine(unarchiveOutputFolder, relativePath);
                Assert.AreEqual(expectedPath, extractedFile.Path);
                Assert.AreEqual(f.Name, extractedFile.Name);
                Assert.AreEqual(f.Content, extractedFile.Content);
            }
        }


    }
}
