using System;
using System.Collections.Generic;
using Lux.IO;

namespace Lux.Tests.IO
{
    public static class DataFactory
    {
        public static FileMock CreateFile(string path)
        {
            var content = $"This is the original content of file: '{path}'";
            var file = FileMock.Create(path, content);
            return file;
        }

        public static FileMock CreateFile(string path, string content)
        {
            var file = FileMock.Create(path, content);
            return file;
        }


        public static IEnumerable<FileMock> CreateFiles(params string[] paths)
        {
            foreach (var path in paths)
            {
                var content = $"This is the original content of file: '{path}'";
                var file = FileMock.Create(path, content);
                yield return file;
            }
        }


        public static IEnumerable<FileMock> SampleFiles()
        {
            var files = new List<FileMock>();
            files.AddRange(CreateJunkFiles());
            files.AddRange(CreatePeopleFiles());
            return files;
        }


        public static IEnumerable<FileMock> CreateJunkFiles()
        {
            var files = CreateFiles("C:/root/sub/child1/info.txt",
                                    "C:/root/sub/child2/info.txt",
                                    "C:/root/sub/file1.txt",
                                    "C:/root.txt");
            return files;
        }

        public static IEnumerable<FileMock> CreatePeopleFiles()
        {
            var files = CreateFiles("C:/names/surname/a/aniston/aniston jennifer.txt",
                                    "C:/names/firstname/b/bill/bill gates.txt",
                                    "C:/names/firstname/t/tom/tom hanks.txt",
                                    "C:/names/firstname/t/tom/tom cruise.txt");
            return files;
        }

    }
}
