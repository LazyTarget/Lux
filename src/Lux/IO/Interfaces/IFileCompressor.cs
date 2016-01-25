namespace Lux.IO
{
    public interface IFileCompressor
    {
        void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName);

        void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName);

        string[] ExtractEntries(string sourceArchiveFileName, string destinationDirectoryName);
    }
}
