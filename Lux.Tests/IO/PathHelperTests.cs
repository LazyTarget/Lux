using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.IO
{
    [TestFixture]
    public class PathHelperTests
    {
        [TestCase]
        [Category("ExcludeBuildAgent")]     // todo: fix so be able to...
        public void GetRootParent()
        {
            const string expected = "C:/";
            const string path1 = "C:/file.txt";
            const string path2 = "C:/dir1/file.txt";
            const string path3 = "C:/directory/subdir/file.txt";
            const string path4 = "C:";

            Assert.AreEqual(expected, PathHelper.GetRootParent(path1));
            Assert.AreEqual(expected, PathHelper.GetRootParent(path2));
            Assert.AreEqual(expected, PathHelper.GetRootParent(path3));
            Assert.AreEqual(expected, PathHelper.GetRootParent(path4));
        }


        [TestCase]
        public void Subtract1()
        {
            const string expected = "file.txt";
            const string x = "C:/dir/subdir/file.txt";
            const string y = "C:/dir/subdir/";
            var actual = PathHelper.Subtract(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void Subtract2()
        {
            const string expected = "file.txt";
            const string x = "C:/dir/subdir/file.txt";
            const string y = "C:/dir/subdir";
            var actual = PathHelper.Subtract(x, y);
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void Subtract3()
        {
            const string expected = "dir/subdir/file.txt";
            const string x = "C:/dir/subdir/file.txt";
            const string y = "C:/";
            var actual = PathHelper.Subtract(x, y);
            Assert.AreEqual(expected, actual);
        }

    }
}
