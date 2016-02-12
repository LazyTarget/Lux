using System;
using System.IO;
using Lux.Extensions;
using Lux.IO;

namespace Lux.Config
{
    public class UriConfigLocation : IConfigLocation
    {
        public UriConfigLocation()
        {
            
        }

        public bool CanRead { get { return Uri != null; } }
        public bool CanWrite { get { return Uri?.IsFile ?? false; } }

        public Uri Uri { get; set; }

        public IFileSystem FileSystem { get; set; }

        public bool CreateNewOnRead { get; set; }


        public virtual Stream GetStreamForRead(IConfigArguments arguments)
        {
            var configUri = Uri;
            if (!configUri.IsAbsoluteUri)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configUri.ToString());
                configUri = new Uri(path);
            }

            if (configUri.IsFile)
            {
                var fileInfo = new FileInfo(configUri.LocalPath);
                if (fileInfo.Exists)
                {
                    var fileStream = FileSystem.OpenFile(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    return fileStream;
                }
                else
                {
                    if (CreateNewOnRead)
                    {
                        var dirPath = PathHelper.GetParent(fileInfo.FullName);
                        if (!FileSystem.DirExists(dirPath))
                            FileSystem.CreateDir(dirPath);
                        var fileStream = FileSystem.OpenFile(fileInfo.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                        return fileStream;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                var client = new System.Net.Http.HttpClient();
                try
                {
                    var task = client.GetAsync(configUri);
                    var response = task.WaitForResult();
                    response.EnsureSuccessStatusCode();

                    var task2 = response.Content.ReadAsStreamAsync();
                    var stream = task2.WaitForResult();
                    return stream;
                }
                catch (Exception ex)
                {
                    throw;
                }
                throw new NotImplementedException("Network paths not implemented");
            }
        }


        public virtual Stream GetStreamForWrite(IConfigArguments arguments)
        {
            var configUri = Uri;
            if (!configUri.IsAbsoluteUri)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configUri.ToString());
                configUri = new Uri(path);
            }

            if (configUri.IsFile)
            {
                var fileInfo = new FileInfo(configUri.LocalPath);
                if (fileInfo.Exists)
                {
                    var fileStream = FileSystem.OpenFile(fileInfo.FullName, FileMode.Truncate, FileAccess.ReadWrite, FileShare.Read);
                    return fileStream;
                }
                else
                {
                    var dirPath = PathHelper.GetParent(fileInfo.FullName);
                    if (!FileSystem.DirExists(dirPath))
                        FileSystem.CreateDir(dirPath);
                    var fileStream = FileSystem.OpenFile(fileInfo.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                    return fileStream;
                }
            }
            else
            {
                throw new NotSupportedException($"Saving against a network path is not supported");
            }
        }

    }
}
