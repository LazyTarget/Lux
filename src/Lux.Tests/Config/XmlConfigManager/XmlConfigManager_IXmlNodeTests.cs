using System;
using System.Xml.Linq;
using Lux.Config;
using Lux.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Config.XmlConfigManager
{
    [TestFixture]
    public class XmlConfigManager_IXmlNodeTests : XmlConfigManagerTestBase
    {
        #region Tests

        [TestCase]
        public void Load()
        {
            var sut = GetSUT();

            var source = new XmlConfigLocation
            {
                Uri = new Uri("C:/app.config"),
                RootElementName = "Root",
            };
            var expected = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(source, sut.FileSystem, expected);

            var canLoad = sut.CanLoad<MyAppConfig>(source);
            Assert.IsTrue(canLoad);

            var actual = sut.Load<MyAppConfig>(source);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AppName, actual.AppName);
            Assert.AreEqual(expected.AppVersion, actual.AppVersion);
        }


        [TestCase]
        public void Save()
        {
            var sut = GetSUT();

            var target = new XmlConfigLocation
            {
                Uri = new Uri("C:/app.config"),
                RootElementName = "Root",
            };
            var expected = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };

            var canSave = sut.CanSave<MyAppConfig>(expected, target);
            Assert.IsTrue(canSave);

            sut.Save<MyAppConfig>(expected, target);
            

            // Assert
            var actual = new MyAppConfig();
            LoadFromFile(target, sut.FileSystem, actual);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.AppName, actual.AppName);
            Assert.AreEqual(expected.AppVersion, actual.AppVersion);
        }


        [TestCase]
        public void LoadEditAndSave()
        {
            var sut = GetSUT();

            // Arrange
            var source = new XmlConfigLocation
            {
                Uri = new Uri("C:/app.config"),
                RootElementName = "Root",
            };
            var original = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(source, sut.FileSystem, original);

            // Act
            var canLoad = sut.CanLoad<MyAppConfig>(source);
            Assert.IsTrue(canLoad);

            var edited = sut.Load<MyAppConfig>(source);

            // Assert
            Assert.IsNotNull(edited);
            Assert.AreEqual(original.AppName, edited.AppName);
            Assert.AreEqual(original.AppVersion, edited.AppVersion);


            // Arrange
            edited.AppVersion = "2.0.0.0";
            
            // Act
            var canSave = sut.CanSave<MyAppConfig>(edited, source);
            Assert.IsTrue(canSave);

            sut.Save<MyAppConfig>(edited, source);


            // Assert
            var actual = new MyAppConfig();
            LoadFromFile(source, sut.FileSystem, actual);

            Assert.IsNotNull(actual);
            Assert.AreEqual(edited.AppName, actual.AppName);
            Assert.AreEqual(edited.AppVersion, actual.AppVersion);
        }


        #endregion


        #region Classes

        public class MyAppConfig : IConfig, IXmlNode, IXmlConfigurable, IXmlExportable
        {
            public IConfigLocation Location { get; set; }

            public string AppName { get; set; }
            public string AppVersion { get; set; }

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
        }

        #endregion

    }
}
