using System.Linq;
using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
    [TestFixture]
    public class ZipFileMockTests
    {
        [TestCase]
        public void Archive()
        {
            const string archiveFileName        = "archive.zip";
            const string archiveOutputPath      = "C:/" + archiveFileName;
            const string archiveFolder          = "C:/folder";
            const string fileName1              = archiveFolder + "/file1.txt";
            const string fileName2              = archiveFolder + "/file2.txt";
            const string fileName3              = archiveFolder + "/subfolder/file3.txt";

            var files = DataFactory.CreateFiles(fileName1,
                                                fileName2,
                                                fileName3).ToList();

            var zipFile = ZipFileMock.Archive(archiveFolder, archiveOutputPath, files.ToArray());
            Assert.IsNotNull(zipFile);
            Assert.AreEqual(archiveFileName, zipFile.Name);
            Assert.AreEqual(archiveOutputPath, zipFile.Path);
            Assert.AreEqual(files.Count, zipFile.FileCount);
        }



        [TestCase]
        public void ArchiveAndUnarchive()
        {
            const string archiveFileName        = "archive.zip";
            const string archiveOutputPath      = "C:/" + archiveFileName;
            const string archiveFolder          = "C:/folder";
            const string fileName1              = archiveFolder + "/file1.txt";
            const string fileName2              = archiveFolder + "/file2.txt";
            const string fileName3              = archiveFolder + "/subfolder/file3.txt";

            var files = DataFactory.CreateFiles(fileName1,
                                                fileName2,
                                                fileName3).ToList();

            var zipFile = ZipFileMock.Archive(archiveFolder, archiveOutputPath, files.ToArray());
            Assert.IsNotNull(zipFile);
            Assert.AreEqual(archiveFileName, zipFile.Name);
            Assert.AreEqual(archiveOutputPath, zipFile.Path);
            Assert.AreEqual(files.Count, zipFile.FileCount);


            const string unarchiveOutputFolder = "C:/output";

            var i = 0;
            var extractedFiles = ZipFileMock.Unarchive(zipFile.Bytes, unarchiveOutputFolder);
            foreach (var extractedFile in extractedFiles)
            {
                var file = files[i++];
                var relativePath = PathHelper.Subtract(file.Path, archiveFolder);
                var expectedPath = PathHelper.Combine(unarchiveOutputFolder, relativePath);
                Assert.AreEqual(expectedPath, extractedFile.Path);
                Assert.AreEqual(file.Name, extractedFile.Name);
                Assert.AreEqual(file.Content, extractedFile.Content);
            }
        }


    }
}
