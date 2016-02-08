using System.Collections.Generic;
using System.Linq;
using Lux.Extensions;
using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
    [TestFixture]
    [Category("ExcludeBuildAgent")]     // todo: fix so be able to...
    public class FileSystemHelperTests
    {
        [TestCase]
        public void DoubleStarReturnsAllFiles()
        {
            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            const string pattern = "C:/root/**";
            var fileSystemHelper = new FileSystemHelper(mock.Object);
            var actual = fileSystemHelper.FindFilesWildcard(pattern);
            var expected = new List<string>
            {
                //"C:/file0.log",
                "C:/root/file1.log",
                "C:/root/a/file2.log",
                "C:/root/a/b/file3.txt",
                "C:/root/a/b/file4.log",
                "C:/root/a/b/c/file5.log",
                "C:/root/a/b/c/file6.txt",
                "C:/root/a/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }


        [TestCase]
        public void DoubleStarWithFilePattern()
        {
            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            const string pattern = "C:/root/**/*.log";
            var fileSystemHelper = new FileSystemHelper(mock.Object);
            var actual = fileSystemHelper.FindFilesWildcard(pattern);
            var expected = new List<string>
            {
                //"C:/file0.log",
                "C:/root/file1.log",
                "C:/root/a/file2.log",
                //"C:/root/a/b/file3.txt",
                "C:/root/a/b/file4.log",
                "C:/root/a/b/c/file5.log",
                //"C:/root/a/b/c/file6.txt",
                "C:/root/a/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        

        [TestCase]
        public void SingleStarWithFilePattern()
        {
            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            const string pattern = "C:/root/*/*.log";
            var fileSystemHelper = new FileSystemHelper(mock.Object);
            var actual = fileSystemHelper.FindFilesWildcard(pattern);
            var expected = new List<string>
            {
                //"C:/file0.log",
                //"C:/root/file1.log",
                "C:/root/a/file2.log",
                //"C:/root/a/b/file3.txt",
                //"C:/root/a/b/file4.log",
                //"C:/root/a/b/c/file5.log",
                //"C:/root/a/b/c/file6.txt",
                //"C:/root/a/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }


        [TestCase]
        public void DoubleStarWithTwoFilePatterns()
        {
            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/b/c/assembly.dll",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            const string pattern = "C:/root/**/*.log|*.txt";
            var fileSystemHelper = new FileSystemHelper(mock.Object);
            var actual = fileSystemHelper.FindFilesWildcard(pattern);
            var expected = new List<string>
            {
                //"C:/file0.log",
                "C:/root/file1.log",
                "C:/root/a/file2.log",
                "C:/root/a/b/file3.txt",
                "C:/root/a/b/file4.log",
                "C:/root/a/b/c/file5.log",
                "C:/root/a/b/c/file6.txt",
                //"C:/root/a/b/c/assembly.dll",
                "C:/root/a/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        
        [TestCase]
        public void SingleStarWithTwoFilePatterns()
        {
            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/b/c/assembly.dll",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            const string pattern = "C:/root/a/*/*.log|*.txt";
            var fileSystemHelper = new FileSystemHelper(mock.Object);
            var actual = fileSystemHelper.FindFilesWildcard(pattern);
            var expected = new List<string>
            {
                //"C:/file0.log",
                //"C:/root/file1.log",
                //"C:/root/a/file2.log",
                "C:/root/a/b/file3.txt",
                "C:/root/a/b/file4.log",
                //"C:/root/a/b/c/file5.log",
                //"C:/root/a/b/c/file6.txt",
                //"C:/root/a/b/c/assembly.dll",
                "C:/root/a/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }



        
        [TestCase]
        public void CopyDir()
        {
            const string sourceDirName          = "C:/root/a";
            const string targetDirName          = "C:/root/x";
            const bool overwrite                = true;

            var files = DataFactory.CreateFiles("C:/file0.log",
                                                "C:/root/file1.log",
                                                "C:/root/a/file2.log",
                                                "C:/root/a/b/file3.txt",
                                                "C:/root/a/b/file4.log",
                                                "C:/root/a/b/c/file5.log",
                                                "C:/root/a/b/c/file6.txt",
                                                "C:/root/a/b/c/assembly.dll",
                                                "C:/root/a/d/file7.log").ToList();

            var mock = FileSystemMock.Create();
            files.ForEach(mock.BaseFileSystemHelper.AddFile);

            var fileSystemHelper = new FileSystemHelper(mock.Object);
            fileSystemHelper.CopyDir(sourceDirName, targetDirName, overwrite);

            var actual = fileSystemHelper.FindFilesWildcard("C:/**");

            var expected = new List<string>
            {
                "C:/file0.log",
                "C:/root/file1.log",
                "C:/root/a/file2.log",
                "C:/root/a/b/file3.txt",
                "C:/root/a/b/file4.log",
                "C:/root/a/b/c/file5.log",
                "C:/root/a/b/c/file6.txt",
                "C:/root/a/b/c/assembly.dll",
                "C:/root/a/d/file7.log",

                "C:/root/x/file2.log",
                "C:/root/x/b/file3.txt",
                "C:/root/x/b/file4.log",
                "C:/root/x/b/c/file5.log",
                "C:/root/x/b/c/file6.txt",
                "C:/root/x/b/c/assembly.dll",
                "C:/root/x/d/file7.log",
            }.OrderBy(new PathSorter()).ToList();

            Assert.AreEqual(expected.Count, actual.Count);
            CollectionAssert.AreEqual(expected, actual);
        }

    }
}
