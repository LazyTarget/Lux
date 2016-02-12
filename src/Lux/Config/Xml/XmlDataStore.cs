using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Lux.Data;
using Lux.Extensions;
using Lux.IO;

namespace Lux.Config.Xml
{
    public class ConfigXmlDataStore : IDataStore<IConfigDescriptor, XDocument>
    {
        public ConfigXmlDataStore()
        {
            FileSystem = new FileSystem();

            XmlReaderSettings = new XmlReaderSettings();
            XmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
            XmlWriterSettings = new XmlWriterSettings();
            XmlWriterSettings.Indent = true;

            CreateNewOnRead = false;
        }

        public Uri Uri { get; set; }
        public IFileSystem FileSystem { get; set; }
        public XmlReaderSettings XmlReaderSettings { get; }
        public XmlWriterSettings XmlWriterSettings { get; }
        public bool CreateNewOnRead { get; set; }


        object IDataStore<IConfigDescriptor>.Load(IConfigDescriptor key)
        {
            return Load(key);
        }

        public XDocument Load(IConfigDescriptor descriptor)
        {
            XDocument document = null;
            using (var stream = GetStreamForRead(descriptor))
            {
                if (stream != null)
                    document = LoadXDocument(stream);
            }
            return document;
        }

        public object Save(IConfigDescriptor descriptor, object value)
        {
            var document = value as XDocument;
            if (document == null)
            {
                var str = value as string;
                if (!string.IsNullOrEmpty(str))
                    document = XDocument.Parse(str);
            }

            var result = Save(descriptor, document);
            return result;
        }

        public XDocument Save(IConfigDescriptor descriptor, XDocument document)
        {
            bool res;
            using (var stream = GetStreamForWrite(descriptor))
            {
                res = SaveXDocument(document, stream);
            }
            return document;
        }
        

        
        protected virtual XDocument LoadXDocument(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            XDocument xdoc = null;
            try
            {
                using (var xmlReader = XmlReader.Create(stream, XmlReaderSettings))
                {
                    if (stream.Length > 0)
                    {
                        xdoc = XDocument.Load(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return xdoc;
        }


        protected virtual bool SaveXDocument(XDocument xdoc, Stream stream)
        {
            if (!stream.CanWrite)
                throw new NotSupportedException("The stream cannot be written to");
            try
            {
                using (var xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
                {
                    xdoc.Save(xmlWriter);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected virtual Stream GetStreamForRead(IConfigDescriptor descriptor)
        {
            //Uri uri = descriptor?.Properties?.GetOrDefault("Uri").Cast<Uri>();
            //if (uri == null)
            //{
            //    throw new ArgumentException("Configuration uri is not defined");
            //}
            Uri uri = Uri;

            if (!uri.IsAbsoluteUri)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uri.ToString());
                uri = new Uri(path);
            }

            if (uri.IsFile)
            {
                var fileInfo = new FileInfo(uri.LocalPath);
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
                    var task = client.GetAsync(uri);
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
        
        protected virtual Stream GetStreamForWrite(IConfigDescriptor descriptor)
        {
            //Uri uri = descriptor?.Properties?.GetOrDefault("Uri").Cast<Uri>();
            //if (uri == null)
            //{
            //    throw new ArgumentException("Configuration uri is not defined");
            //}
            Uri uri = Uri;

            if (!uri.IsAbsoluteUri)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uri.ToString());
                uri = new Uri(path);
            }

            if (uri.IsFile)
            {
                var fileInfo = new FileInfo(uri.LocalPath);
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