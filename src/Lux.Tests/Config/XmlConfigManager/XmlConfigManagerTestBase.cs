﻿using System.IO;
using System.Xml.Linq;
using Lux.Config;
using Lux.IO;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Tests.Config.XmlConfigManager
{
    public abstract class XmlConfigManagerTestBase : TestBase
    {
        protected virtual TestableXmlConfigManager GetSUT()
        {
            return new TestableXmlConfigManager();
        }

        
        #region Helpers

        
        protected void LoadFromFile(XmlConfigSource source, IFileSystem fileSystem, IXmlConfigurable configurable)
        {
            XDocument xdocument;
            var fileName = source.Uri.LocalPath;
            if (fileSystem.FileExists(fileName))
            {
                using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    xdocument = XDocument.Load(stream);
                }
            }
            else
                throw new FileNotFoundException("The requested file was not found", fileName);

            var rootElement = xdocument.GetOrCreateElement(source.RootElementName);
            configurable.Configure(rootElement);
        }

        protected void SaveToFile(XmlConfigSource target, IFileSystem fileSystem, IXmlExportable exportable)
        {
            var xdocument = new XDocument();
            var rootElement = xdocument.GetOrCreateElement(target.RootElementName);
            exportable.Export(rootElement);

            var fileName = target.Uri.LocalPath;
            var dirPath = PathHelper.GetParent(fileName);
            if (!fileSystem.DirExists(dirPath))
                fileSystem.CreateDir(dirPath);
            using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                xdocument.Save(stream);
            }
        }

        #endregion


        #region Classes
        
        public class TestableXmlConfigManager : Lux.Config.XmlConfigManager
        {
            public readonly IFileSystem FileSystem = new MemoryFileSystem();


            protected override Stream GetStreamFromSource(ConfigSource source)
            {
                var xmlConfigSource = (XmlConfigSource) source;
                var fileName = xmlConfigSource.Uri.LocalPath;
                var exists = FileSystem.FileExists(fileName);
                Stream stream = null;
                if (exists)
                {
                    // todo: truncate?
                    stream = FileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    var dirPath = PathHelper.GetParent(fileName);
                    if (!FileSystem.DirExists(dirPath))
                        FileSystem.CreateDir(dirPath);
                    stream = FileSystem.OpenFile(fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                }
                //stream = base.GetStreamFromSource(source);
                return stream;
            }
        }

        #endregion

    }
}
