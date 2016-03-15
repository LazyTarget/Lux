using System;
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
    public class XmlConfigManager_IXmlNodeTests : XmlConfigManagerTestBase
    {
        protected override void SetUp()
        {
            FileSystem = new MemoryFileSystem();

            base.SetUp();
        }

        protected override TestableXmlConfigManager GetSUT()
        {
            var sut = base.GetSUT();
            sut.DefaultDescriptorFactory = null;
            return sut;
        }


        #region Tests

        [TestCase]
        public void Load()
        {
            var sut = GetSUT();

            var source = new XmlConfigDescriptor
            {
                Uri = new Uri("C:/app.config"),
                RootElementPath = "Root",
            };
            var expected = new MyConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(source, FileSystem, expected);

            var canLoad = sut.CanLoad<MyConfig>(source);
            Assert.IsTrue(canLoad);

            var actual = sut.Load<MyConfig>(source);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AppName, actual.AppName);
            Assert.AreEqual(expected.AppVersion, actual.AppVersion);
        }


        [TestCase]
        public void Save()
        {
            var sut = GetSUT();

            var target = new XmlConfigDescriptor
            {
                Uri = new Uri("C:/app.config"),
                RootElementPath = "Root",
            };
            target.DataStore = new ConfigXmlDataStore
            {
                Uri = target.Uri,
                FileSystem = FileSystem,
            };

            var expected = new MyConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };

            var canSave = sut.CanSave<MyConfig>(expected, target.DataStore);
            Assert.IsTrue(canSave);

            sut.Save<MyConfig>(expected, target.DataStore);
            

            // Assert
            var actual = new MyConfig();
            LoadFromFile(target, FileSystem, actual);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AppName, actual.AppName);
            Assert.AreEqual(expected.AppVersion, actual.AppVersion);
        }


        [TestCase]
        public void LoadEditAndSave()
        {
            var sut = GetSUT();

            // Arrange
            var source = new XmlConfigDescriptor
            {
                Uri = new Uri("C:/app.config"),
                RootElementPath = "Root",
            };
            source.DataStore = new ConfigXmlDataStore
            {
                Uri = source.Uri,
                FileSystem = FileSystem,
            };

            var original = new MyConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(source, FileSystem, original);

            // Act
            var canLoad = sut.CanLoad<MyConfig>(source);
            Assert.IsTrue(canLoad);

            var edited = sut.Load<MyConfig>(source);

            // Assert
            Assert.IsNotNull(edited);
            Assert.AreEqual(original.AppName, edited.AppName);
            Assert.AreEqual(original.AppVersion, edited.AppVersion);


            // Arrange
            edited.AppVersion = "2.0.0.0";
            
            // Act
            var canSave = sut.CanSave<MyConfig>(edited, source.DataStore);
            Assert.IsTrue(canSave);

            sut.Save<MyConfig>(edited, source.DataStore);


            // Assert
            var actual = new MyConfig();
            LoadFromFile(source, FileSystem, actual);

            Assert.IsNotNull(actual);
            Assert.AreEqual(edited.AppName, actual.AppName);
            Assert.AreEqual(edited.AppVersion, actual.AppVersion);
        }


        #endregion


        #region Classes

        public class MyConfig : XmlConfigBase //IConfig, IXmlNode, IXmlConfigurable, IXmlExportable
        {
            //public IConfigDescriptor Descriptor { get; set; }

            public string AppName { get; set; }
            public string AppVersion { get; set; }

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
