using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
    [TestFixture]
    public class MockFileSystemTests
    {
        [TestCase]
        public void DifferntPathTypesEqualityComparer()
        {
            const string fileName1 = "C:/file.txt";
            const string fileName2 = "C:\\file.txt";
            const string fileName3 = " C:\\file.txt ";

            var mock = FileSystemMock.Create();
            //mock.Files.Add(fileName1, null);
            //Assert.IsTrue(mock.Files.ContainsKey(fileName1));
            //Assert.IsTrue(mock.Files.ContainsKey(fileName2));
            //Assert.IsTrue(mock.Files.ContainsKey(fileName3));
            mock.BaseFileSystemHelper.AddFile(FileMock.Create(fileName1));
            Assert.IsNotNull(mock.BaseFileSystemHelper.GetFileOrDefault(fileName1));
            Assert.IsNotNull(mock.BaseFileSystemHelper.GetFileOrDefault(fileName2));
            Assert.IsNotNull(mock.BaseFileSystemHelper.GetFileOrDefault(fileName3));
        }


        [TestCase]
        public void OpenWhenFileDoesntExist()
        {
            const string fileName = "C:/file.txt";

            var files = DataFactory.SampleFiles().ToList();
            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            Assert.IsNull(mock.BaseFileSystemHelper.GetFileOrDefault(fileName));
            var fileSystem = mock.Object;

            Action act = () => fileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read);
            //act.AssertThrows<FileNotFoundException>(Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND);
            Assert.Throws<FileNotFoundException>(new TestDelegate(act), Lux.IO.Consts.ErrorMessages.ERROR_FILE_NOT_FOUND);
        }


        [TestCase]
        public void OpenFileAssertContent()
        {
            const string fileName = "C:/file.txt";
            var content = string.Format("Some content, row 1{0}Row 2{0}Some new row{0}Row 4", Environment.NewLine);

            var mock = FileSystemMock.Create();
            //mock.Files.Add(fileName, FileMock.Create(fileName, content));
            mock.BaseFileSystemHelper.AddFile(FileMock.Create(fileName, content));

            var fileSystem = mock.Object;
            var stream = fileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var actual = streamReader.ReadToEnd();
            Assert.AreEqual(content, actual);
        }


        [TestCase]
        public void CopyChangeContentVerifyDifferent()
        {
            const string fileName = "C:/file.txt";
            const string fileName2 = "C:/file2.txt";
            var content = string.Format("Some content, row 1{0}Row 2{0}Some new row{0}Row 4", Environment.NewLine);
            var newLine = string.Format("{0}Added row (5)", Environment.NewLine);

            var mock = FileSystemMock.Create();
            //mock.Files.Add(fileName, FileMock.Create(fileName, content));
            mock.BaseFileSystemHelper.AddFile(FileMock.Create(fileName, content));
            var fileSystem = mock.Object;
            
            // Copy file
            fileSystem.CopyFile(fileName, fileName2, false);
            
            // Update copied file
            var stream = fileSystem.OpenFile(fileName2, FileMode.Append, FileAccess.Write);
            var streamWriter = new StreamWriter(stream, Encoding.UTF8);
            streamWriter.Write(newLine);
            streamWriter.Flush();

            // Verfiy old file has the original content
            stream = fileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            var actual = streamReader.ReadToEnd();
            Assert.AreEqual(content, actual);

            // Verfiy new file has the updated content
            stream = fileSystem.OpenFile(fileName2, FileMode.Open, FileAccess.Read);
            streamReader = new StreamReader(stream, Encoding.UTF8);
            actual = streamReader.ReadToEnd();
            Assert.AreEqual(content + newLine, actual);
        }


        [TestCase]
        public void VerifyFileDoesntExistAfterMove()
        {
            const string fileName = "C:/file.txt";
            const string fileName2 = "C:/file2.txt";
            var content = string.Format("Sample content{0}", Environment.NewLine);

            var mock = FileSystemMock.Create();
            //mock.Files.Add(fileName, FileMock.Create(fileName, content));
            mock.BaseFileSystemHelper.AddFile(FileMock.Create(fileName, content));
            var fileSystem = mock.Object;

            // Move file
            fileSystem.MoveFile(fileName, fileName2);

            Assert.IsFalse(fileSystem.FileExists(fileName), "The move source should not exist");
            Assert.IsTrue(fileSystem.FileExists(fileName2), "The move target seems to be missing");
        }



        [TestCase]
        public void AddFile()
        {
            var file1 = DataFactory.CreateFile("C:/root/sub/file1.txt");
            var file2 = DataFactory.CreateFile("C:/file2.txt");

            var mock = FileSystemMock.Create();
            mock.BaseFileSystemHelper.AddFile(file1);
            mock.BaseFileSystemHelper.AddFile(file2);

            Assert.AreSame(file1, mock.BaseFileSystemHelper.GetFile(file1.Path));
            Assert.AreSame(file2, mock.BaseFileSystemHelper.GetFile(file2.Path));
        }



        [TestCase]
        public void FileMoveWithRename()
        {
            const string sourcePath = "C:/root/sub/file1.txt";
            const string targetPath = "C:/root/child/file2.txt";
            var file = DataFactory.CreateFile(sourcePath);

            var mock = FileSystemMock.Create();
            mock.BaseFileSystemHelper.AddFile(file);

            var fileSystem = mock.Object;
            fileSystem.CreateDir(PathHelper.GetParent(targetPath));
            fileSystem.MoveFile(sourcePath, targetPath);

            Assert.IsFalse(fileSystem.FileExists(sourcePath));
            Assert.IsTrue(fileSystem.FileExists(targetPath));
            Assert.AreSame(file, mock.BaseFileSystemHelper.GetFile(targetPath));
        }

        

        [TestCase]
        public void FileCopyWithRename()
        {
            const string sourcePath = "C:/root/sub/file1.txt";
            const string targetPath = "C:/root/child/file2.txt";
            var file = DataFactory.CreateFile(sourcePath);

            var mock = FileSystemMock.Create();
            mock.BaseFileSystemHelper.AddFile(file);

            var fileSystem = mock.Object;
            fileSystem.CreateDir(PathHelper.GetParent(targetPath));
            fileSystem.CopyFile(sourcePath, targetPath, false);

            Assert.IsTrue(fileSystem.FileExists(sourcePath));
            Assert.IsTrue(fileSystem.FileExists(targetPath));
            Assert.AreSame(file, mock.BaseFileSystemHelper.GetFile(sourcePath));

            var targetFile = mock.BaseFileSystemHelper.GetFile(targetPath);
            Assert.AreNotSame(file, targetFile);
            Assert.AreEqual(file.Content, targetFile.Content);
        }


        [TestCase]
        public void EnumerateFiles_AllDirectories()
        {
            var files = DataFactory.SampleFiles().ToList();
            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);
            
            var fileSystem = mock.Object;
            var actual = fileSystem.EnumerateFiles("C:/", null, SearchOption.AllDirectories).ToList();
            var expected = files.Select(x => x.Path).OrderBy(x => x, new PathComparer()).ToList();
            CollectionAssert.AreEquivalent(expected, actual);
        }


        [TestCase]
        public void EnumerateFiles_TopDirectoryOnly()
        {
            var files = DataFactory.SampleFiles().ToList();
            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            var fileSystem = mock.Object;
            var actual = fileSystem.EnumerateFiles("C:/names/firstname/t/tom", null, SearchOption.TopDirectoryOnly).ToList();
            var expected = new List<string>
            {
                "C:/names/firstname/t/tom/tom cruise.txt",
                "C:/names/firstname/t/tom/tom hanks.txt",
            };
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
