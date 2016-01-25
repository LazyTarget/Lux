using System.IO.Compression;
using System.Linq;

namespace Lux.IO
{
    public class ZipFileCompressor : IFileCompressor
    {
        public void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName)
        {
            ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName);
        }

        public void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
        {
            ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        }

        public string[] ExtractEntries(string sourceArchiveFileName, string destinationDirectoryName)
        {
            using (var archive = ZipFile.OpenRead(sourceArchiveFileName))
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                var entries = archive.Entries.Select(x => x.FullName).ToArray();
                return entries;
            }
        }
    }
}
