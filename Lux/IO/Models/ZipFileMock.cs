using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Lux.IO
{
    public class ZipFileMock : FileMock
    {
        private readonly FileMock[] _files;

        private ZipFileMock(string path, byte[] content, params FileMock[] files)
            : base(path, content)
        {
            _files = files ?? new FileMock[0];
        }

        public int FileCount
        {
            get { return _files.Length; }
        }
        

        public override string ToString()
        {
            return Path;
            //return Name;
            //return Content;
        }




        
        public static ZipFileMock Archive(string sourceDirectoryName, string destinationArchiveFileName, params FileMock[] files)
        {
            var bytes = new byte[0];
            using (var stream = new MemoryStream())
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var relativePath = PathHelper.Subtract(file.Path, sourceDirectoryName);
                        var entry = archive.CreateEntry(relativePath);

                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(file.Bytes, 0, file.Bytes.Length);
                            entryStream.Flush();
                        }
                    }
                }

                // Fix for "invalid zip archive"
                using ( var fileStream = new MemoryStream() )
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                    bytes = fileStream.ToArray();
                }
            }

            var zipFile = new ZipFileMock(destinationArchiveFileName, bytes, files);
            return zipFile;
        }


        public static IEnumerable<FileMock> Unarchive(byte[] zipFileBytes, string unarchiveTarget)
        {
            using(var stream = new MemoryStream(zipFileBytes, false))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                foreach (var entry in archive.Entries)
                {
                    using (var entryStream = entry.Open())
                    {
                        var bytes = entryStream.ReadAllAsBytes();
                        var relativePath = entry.FullName;
                        var filePath = PathHelper.Combine(unarchiveTarget, relativePath);
                        var file = Create(filePath, bytes);
                        yield return file;
                    }
                }
            }
        }

    }
}
