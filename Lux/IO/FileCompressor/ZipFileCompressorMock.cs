using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Lux.IO
{
    public class ZipFileCompressorMock : IFileCompressor
    {
        private readonly IFileSystem _fileSystem;
        private readonly FileSystemHelper _fileSystemHelper;

        public ZipFileCompressorMock(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _fileSystemHelper = new FileSystemHelper(_fileSystem);
        }


        void IFileCompressor.CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
        {
            var zipFile = CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
        }

        public ZipFileMock CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
        {
            var files = _fileSystemHelper.EnumerateFiles(sourceDirectoryName, null, SearchOption.AllDirectories, FileShare.ReadWrite).ToArray();

            ZipFileMock zipFile;
            using (var stream = _fileSystem.OpenFile(destinationArchiveFileName, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                zipFile = ZipFileMock.Archive(sourceDirectoryName, destinationArchiveFileName, files);
                stream.Write(zipFile.Bytes, 0, zipFile.Bytes.Length);
                stream.Flush();
            }
            return zipFile;
        }



        void IFileCompressor.ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
        {
            var files = ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        }

        public IEnumerable<FileMock> ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
        {
            using (var stream = _fileSystem.OpenFile(sourceArchiveFileName, FileMode.Open, FileAccess.Read))
            {
                var bytes = stream.ReadAllAsBytes();
                var files = ZipFileMock.Unarchive(bytes, destinationDirectoryName).ToList();
                foreach (var file in files)
                {
                    _fileSystemHelper.AddFile(file);
                }
                return files;
            }
        }



        public string[] ExtractEntries(string sourceArchiveFileName, string destinationDirectoryName)
        {
            using (var archiveStream = _fileSystem.OpenFile(sourceArchiveFileName, FileMode.Open, FileAccess.Read))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    var path = PathHelper.Combine(destinationDirectoryName, entry.FullName);
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        // is a directory
                        if (!_fileSystem.DirExists(path))
                            _fileSystem.CreateDir(path);
                        continue;
                    }
                    else
                    {
                        var dir = PathHelper.GetParent(path);
                        if (!_fileSystem.DirExists(dir))
                            _fileSystem.CreateDir(dir);
                    }

                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = _fileSystem.OpenFile(path, FileMode.Create, FileAccess.Write))
                        {
                            entryStream.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                    }
                }
                var entries = archive.Entries.Select(x => x.FullName).ToArray();
                return entries;
            }
        }

    }
}
