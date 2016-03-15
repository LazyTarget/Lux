using System;
using System.IO;
using System.Xml.Linq;
using Lux.Config;
using Lux.Config.Xml;
using Lux.IO;
using Lux.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Config.XmlConfigManager
{
    [TestFixture]
    public abstract class XmlConfigManagerTestBase : TestBase
    {
        protected IFileSystem FileSystem;

        protected XmlConfigManagerTestBase()
        {
            FileSystem = new MemoryFileSystem();
            //FileSystem = new FileSystem();

            Framework.ConfigurationManager = new LuxConfigurationManager(new ConfigurationManagerAdapter())
            {
                FileSystem = FileSystem,
            };
        }

        protected virtual TestableXmlConfigManager GetSUT()
        {
            var sut = new TestableXmlConfigManager();
            sut.DefaultDescriptorFactory = new AppConfigDescriptorFactory
            {
                ConfigurationManager = Framework.ConfigurationManager,
            };
            return sut;
        }

        
        #region Helpers


        protected void SaveAppConfigFile(string xml, IFileSystem fileSystem)
        {
            xml = xml.Trim();
            var document = XDocument.Parse(xml);
            SaveAppConfigFile(document, fileSystem);
        }
        
        protected void SaveAppConfigFile(XDocument document, IFileSystem fileSystem)
        {
            var fileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;


            var dirPath = PathHelper.GetParent(fileName);
            if (!fileSystem.DirExists(dirPath))
                fileSystem.CreateDir(dirPath);

            var exists = fileSystem.FileExists(fileName);
            var fileMode = exists ? FileMode.Truncate : FileMode.CreateNew;
            var fileAccess = FileAccess.ReadWrite;
            var fileShare = FileShare.Read;
            using (var stream = fileSystem.OpenFile(fileName, fileMode, fileAccess, fileShare))
            {
                document.Save(stream);
            }
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
                    ConfigurationManager = Framework.ConfigurationManager,
                };
            }
        }

        public class CustomXmlConfig : XmlConfigBase, IEquatable<CustomXmlConfig>
        {
            //public IConfigDescriptor Descriptor { get; set; }


            public string AppName { get; set; }
            public string AppVersion { get; set; }


            public bool Equals(CustomXmlConfig other)
            {
                if (other == null)
                    return false;

                if (!string.Equals(AppName, other.AppName))
                    return false;
                if (!string.Equals(AppVersion, other.AppVersion))
                    return false;
                return true;
            }

            public override bool Equals(object obj)
            {
                var eq = base.Equals(obj);
                if (!eq)
                {
                    if (obj is CustomXmlConfig)
                        eq = Equals((CustomXmlConfig) obj);
                }
                return eq;
            }


            public override void Configure(XElement element)
            {
                var elem = element.Element(nameof(AppName));
                if (elem != null)
                {
                    AppName = elem.Value;
                }

                elem = element.Element(nameof(AppVersion));
                if (elem != null)
                {
                    AppVersion = elem.Value;
                }
            }

            public override void Export(XElement element)
            {
                element.GetOrCreateElement(nameof(AppName)).Value = AppName;
                element.GetOrCreateElement(nameof(AppVersion)).Value = AppVersion;
            }
            
        }

        #endregion

    }
}
