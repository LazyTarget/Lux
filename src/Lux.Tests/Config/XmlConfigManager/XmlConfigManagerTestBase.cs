using System.IO;
using System.Xml.Linq;
using Lux.Config;
using Lux.Config.Xml;
using Lux.IO;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Tests.Config.XmlConfigManager
{
    public abstract class XmlConfigManagerTestBase : TestBase
    {
        protected IFileSystem FileSystem;

        protected XmlConfigManagerTestBase()
        {
            FileSystem = new MemoryFileSystem();
            //FileSystem = new FileSystem();
        }

        protected virtual TestableXmlConfigManager GetSUT()
        {
            var sut = new TestableXmlConfigManager();
            sut.DefaultDescriptorFactory = new AppConfigDescriptorFactory
            {
                FileSystem = FileSystem,
            };
            return sut;
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

        protected void LoadFromFile(XmlConfigDescriptor descriptor, IFileSystem fileSystem, IXmlConfigurable configurable)
        {
            XDocument xdocument;
            var fileName = descriptor.Uri.LocalPath;
            if (fileSystem.FileExists(fileName))
            {
                using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    xdocument = XDocument.Load(stream);
                }
            }
            else
                throw new FileNotFoundException("The requested file was not found", fileName);
            
            var path = descriptor.RootElementPath;
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
        
        protected void SaveToFile(XmlConfigDescriptor descriptor, IFileSystem fileSystem, IXmlExportable exportable, bool replace = false)
        {
            XDocument xdocument;
            var fileName = descriptor.Uri.LocalPath;
            if (!replace && fileSystem.FileExists(fileName))
            {
                using (var stream = fileSystem.OpenFile(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    xdocument = XDocument.Load(stream);
                }
            }
            else
                xdocument = new XDocument();

            var path = descriptor.RootElementPath;
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

        protected XmlConfigDescriptor GetAppConfigDescriptor<TConfig>()
            where TConfig : IConfig
        {
            var factory = new AppConfigDescriptorFactory();
            var descriptor = factory.CreateDescriptor<TConfig>();
            var result = (XmlConfigDescriptor) descriptor;
            return result;
        }

        #endregion


        #region Classes

        public class TestableXmlConfigManager : Lux.Config.Xml.XmlConfigManager
        {
            public TestableXmlConfigManager()
            {
                DefaultDescriptorFactory = new AppConfigDescriptorFactory
                {
                    FileSystem = new MemoryFileSystem(),
                };
            }
        }

        #endregion

    }
}
