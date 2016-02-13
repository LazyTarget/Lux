using System;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Lux.Config;
using Lux.Config.Xml;
using Lux.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Config.XmlConfigManager
{
    [TestFixture]
    public class XmlConfigManager_AppConfigDescriptorTests : XmlConfigManagerTestBase
    {
        protected override void SetUp()
        {
            base.SetUp();
            
            //FileSystem = new FileSystem();
        }

        protected override TestableXmlConfigManager GetSUT()
        {
            var sut = base.GetSUT();
            sut.DefaultDescriptorFactory = new AppConfigDescriptorFactory
            {
                FileSystem = FileSystem,
            };
            return sut;
        }


        #region Tests

        [TestCase]
        public void Load()
        {
            var sut = GetSUT();

            var expectedDescriptor = GetAppConfigDescriptor<MyAppConfig>();
            var expectedConfig = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(expectedDescriptor, FileSystem, expectedConfig);
            
            var canLoad = sut.CanLoad<MyAppConfig>(expectedDescriptor);
            Assert.IsTrue(canLoad);

            var actualConfig = sut.Load<MyAppConfig>(expectedDescriptor);
            Assert.IsNotNull(actualConfig);
            Assert.AreNotSame(expectedConfig, actualConfig);
            Assert.AreEqual(expectedConfig, actualConfig);

            CollectionAssert.AreEquivalent(expectedDescriptor.Properties, actualConfig.Descriptor.Properties);
            var actualDescriptor = (XmlConfigDescriptor) actualConfig.Descriptor;
            Assert.AreEqual(expectedDescriptor.Uri, actualDescriptor.Uri);
            Assert.AreEqual(expectedDescriptor.RootElementPath, actualDescriptor.RootElementPath);
        }


        [TestCase]
        public void SaveWithNullLocation_ShouldDefaultToAppConfig()
        {
            var sut = GetSUT();

            var expectedLocation = GetAppConfigLocation<MyAppConfig>();
            var expectedConfig = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };

            var canSave = sut.CanSave<MyAppConfig>(expectedConfig, dataStore: null);
            Assert.IsTrue(canSave);

            sut.Save<MyAppConfig>(expectedConfig, dataStore: null);
            

            // Assert
            var actualConfig = new MyAppConfig();
            LoadFromFile(expectedLocation, FileSystem, actualConfig);

            Assert.IsNotNull(actualConfig);
            Assert.AreNotSame(expectedConfig, actualConfig);
            Assert.AreEqual(expectedConfig, actualConfig);
        }


        [TestCase]
        public void LoadEditAndSave()
        {
            var sut = GetSUT();

            // Arrange
            var expectedDescriptor = GetAppConfigDescriptor<MyAppConfig>();
            var originalConfig = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(expectedDescriptor, FileSystem, originalConfig);

            // Act
            var canLoad = sut.CanLoad<MyAppConfig>(descriptor: null);
            Assert.IsTrue(canLoad);
            var loadedConfig = sut.Load<MyAppConfig>(descriptor: null);

            // Assert
            Assert.IsNotNull(loadedConfig);
            Assert.AreNotSame(originalConfig, loadedConfig);
            Assert.AreEqual(originalConfig, loadedConfig);

            var loadedDescriptor = (XmlConfigDescriptor)loadedConfig.Descriptor;
            Assert.AreEqual(expectedDescriptor.Uri, loadedDescriptor.Uri);
            Assert.AreEqual(expectedDescriptor.RootElementPath, loadedDescriptor.RootElementPath);


            // Arrange
            var editedConfig = loadedConfig;
            editedConfig.AppVersion = "2.0.0.0";
            
            // Act
            var canSave = sut.CanSave<MyAppConfig>(editedConfig, dataStore: null);
            Assert.IsTrue(canSave);
            sut.Save<MyAppConfig>(editedConfig, dataStore: null);


            // Assert
            var actualConfig = new MyAppConfig();
            LoadFromFile(expectedDescriptor, FileSystem, actualConfig);

            Assert.IsNotNull(actualConfig);
            Assert.AreNotSame(editedConfig, actualConfig);
            Assert.AreEqual(editedConfig, actualConfig);
        }


        #endregion
        

        #region Classes

        public class MyAppConfig : IConfig, IXmlNode, IXmlConfigurable, IXmlExportable, IConfigurationSectionHandler, IEquatable<MyAppConfig>
        {
            public IConfigDescriptor Descriptor { get; set; }


            public string AppName { get; set; }
            public string AppVersion { get; set; }


            public bool Equals(MyAppConfig other)
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
                    if (obj is MyAppConfig)
                        eq = Equals((MyAppConfig) obj);
                }
                return eq;
            }

            public void Configure(XElement element)
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

            public void Export(XElement element)
            {
                element.GetOrCreateElement(nameof(AppName)).Value = AppName;
                element.GetOrCreateElement(nameof(AppVersion)).Value = AppVersion;
            }


            public object Create(object parent, object configContext, XmlNode section)
            {
                return section;
            }
        }

        #endregion

    }
}
