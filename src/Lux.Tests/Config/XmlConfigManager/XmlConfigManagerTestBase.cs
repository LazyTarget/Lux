using System.IO;
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

        
        protected void LoadFromFile(IXmlConfigLocation location, IFileSystem fileSystem, IXmlConfigurable configurable)
        {
            XDocument xdocument;
            var fileName = location.Uri.LocalPath;
            if (fileSystem.FileExists(fileName))
            {
                using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    xdocument = XDocument.Load(stream);
                }
            }
            else
                throw new FileNotFoundException("The requested file was not found", fileName);
            
            var path = location.RootElementPath ?? location.RootElementName;
            var rootElement = xdocument.GetOrCreateElementAtPath(path);
            configurable.Configure(rootElement);
        }

        protected void SaveToFile(IXmlConfigLocation location, IFileSystem fileSystem, IXmlExportable exportable, bool replace = false)
        {
            XDocument xdocument;
            var fileName = location.Uri.LocalPath;
            if (!replace && fileSystem.FileExists(fileName))
            {
                using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    xdocument = XDocument.Load(stream);
                }
            }
            else
                xdocument = new XDocument();

            var path = location.RootElementPath ?? location.RootElementName;
            var rootElement = xdocument.GetOrCreateElementAtPath(path);
            exportable.Export(rootElement);
            
            var dirPath = PathHelper.GetParent(fileName);
            if (!fileSystem.DirExists(dirPath))
                fileSystem.CreateDir(dirPath);
            using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                xdocument.Save(stream);
            }
        }
        
        protected IXmlConfigLocation GetAppConfigLocation<TConfig>()
            where TConfig : IConfig
        {
            var factory = new AppConfigLocationFactory();
            var location = factory.CreateLocation<TConfig>();
            var result = (IXmlConfigLocation) location;
            return result;
        }

        #endregion


        #region Classes
        
        public class TestableXmlConfigManager : Lux.Config.XmlConfigManager
        {
            public TestableXmlConfigManager()
            {
                FileSystem = new MemoryFileSystem();
            }


            protected override Stream GetStreamForRead(IConfigLocation location)
            {
                Stream stream = null;
                stream = base.GetStreamForRead(location);
                return stream;

                var xmlConfigLocation = (IXmlConfigLocation) location;
                var fileName = xmlConfigLocation.Uri.LocalPath;
                var exists = FileSystem.FileExists(fileName);
                if (exists)
                {
                    stream = FileSystem.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    var dirPath = PathHelper.GetParent(fileName);
                    if (!FileSystem.DirExists(dirPath))
                        FileSystem.CreateDir(dirPath);
                    stream = FileSystem.OpenFile(fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                }
                return stream;
            }

            protected override Stream GetStreamForWrite(IConfigLocation location)
            {
                Stream stream = null;
                stream = base.GetStreamForWrite(location);
                return stream;

                var xmlConfigLocation = (IXmlConfigLocation) location;
                var fileName = xmlConfigLocation.Uri.LocalPath;
                var exists = FileSystem.FileExists(fileName);
                if (exists)
                {
                    stream = FileSystem.OpenFile(fileName, FileMode.Truncate, FileAccess.ReadWrite, FileShare.Read);
                }
                else
                {
                    var dirPath = PathHelper.GetParent(fileName);
                    if (!FileSystem.DirExists(dirPath))
                        FileSystem.CreateDir(dirPath);
                    stream = FileSystem.OpenFile(fileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                }
                //stream = base.GetStreamFromLocation(location);
                return stream;
            }
        }

        #endregion

    }
}
