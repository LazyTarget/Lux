using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
#if INTEGRATION_TESTS

#else
    [Ignore("Integration tests disabled")]
#endif

    [TestFixture]
    [Category("IntegrationTest")]
    public class DirectoryManagerWithRealFileSystem_Tests
    {
        [TestCase]
        public void GetStructure_DriveC()
        {
            const string dirPath = "C:/";
            var fileSystem = new FileSystem();
            var sut = new FileSystemHelper(fileSystem);
            var structure = sut.GetStructure(dirPath);

            Assert.IsNotNull(structure);
            Assert.AreEqual(1, structure.Count);
            Assert.IsTrue(structure.ContainsKey(dirPath));
        }


        [TestCase]
        public void GetStructure_WindowsUsersFolder()
        {
            const string dirPath = "C:/Users";
            var fileSystem = new FileSystem();
            var sut = new FileSystemHelper(fileSystem);
            var structure = sut.GetStructure(dirPath);

            Assert.IsNotNull(structure);
            Assert.AreEqual(1, structure.Count);
            Assert.IsTrue(structure.ContainsKey(dirPath));
        }




        //[TestCase]
        //public void GetDirectory_WindowsUsersFolder()
        //{
        //    const string dirPath = "C:/Users";
        //    var fileSystem = new FileSystem();
        //    var sut = new FileSystemHelper(fileSystem);
        //    var directory = sut.GetDirectory(dirPath);

        //    Assert.IsNotNull(directory);
        //    Assert.AreEqual("Users", directory.Name);
        //    Assert.AreEqual(dirPath, directory.Path);
        //}

    }
}
