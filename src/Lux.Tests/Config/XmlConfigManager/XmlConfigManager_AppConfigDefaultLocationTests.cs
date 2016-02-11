using System;
using System.Xml.Linq;
using Lux.Config;
using Lux.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Config.XmlConfigManager
{
    [TestFixture]
    public class XmlConfigManager_AppConfigDefaultLocationTests : XmlConfigManagerTestBase
    {
        protected override TestableXmlConfigManager GetSUT()
        {
            var sut = base.GetSUT();
            sut.DefaultLocationFactory = new AppConfigLocationFactory();
            return sut;
        }


        #region Tests

        [TestCase]
        public void LoadWithNullLocation_ShouldDefaultToAppConfig()
        {
            var sut = GetSUT();

            var expectedLocation = GetAppConfigLocation<MyAppConfig>();
            var expectedConfig = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(expectedLocation, sut.FileSystem, expectedConfig);
            
            var canLoad = sut.CanLoad<MyAppConfig>(location: null);
            Assert.IsTrue(canLoad);

            var actualConfig = sut.Load<MyAppConfig>(location: null);
            Assert.IsNotNull(actualConfig);
            Assert.AreEqual(expectedConfig.AppName, actualConfig.AppName);
            Assert.AreEqual(expectedConfig.AppVersion, actualConfig.AppVersion);

            var actualLocation = (IXmlConfigLocation) actualConfig.Location;
            Assert.AreEqual(expectedLocation.Uri, actualLocation.Uri);
            Assert.AreEqual(expectedLocation.RootElementName, actualLocation.RootElementName);
            Assert.AreEqual(expectedLocation.RootElementPath, actualLocation.RootElementPath);
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

            var canSave = sut.CanSave<MyAppConfig>(expectedConfig, location: null);
            Assert.IsTrue(canSave);

            sut.Save<MyAppConfig>(expectedConfig, location: null);
            

            // Assert
            var actualConfig = new MyAppConfig();
            LoadFromFile(expectedLocation, sut.FileSystem, actualConfig);

            Assert.IsNotNull(actualConfig);
            Assert.AreEqual(expectedConfig.AppName, actualConfig.AppName);
            Assert.AreEqual(expectedConfig.AppVersion, actualConfig.AppVersion);
        }


        [TestCase]
        public void LoadEditAndSave()
        {
            var sut = GetSUT();

            // Arrange
            var expectedLocation = GetAppConfigLocation<MyAppConfig>();
            var originalConfig = new MyAppConfig
            {
                AppName = "MyAppName",
                AppVersion = "1.0.0.0",
            };
            SaveToFile(expectedLocation, sut.FileSystem, originalConfig);

            // Act
            var canLoad = sut.CanLoad<MyAppConfig>(location: null);
            Assert.IsTrue(canLoad);
            var loadedConfig = sut.Load<MyAppConfig>(location: null);

            // Assert
            Assert.IsNotNull(loadedConfig);
            Assert.AreEqual(originalConfig.AppName, loadedConfig.AppName);
            Assert.AreEqual(originalConfig.AppVersion, loadedConfig.AppVersion);

            var loadedLocation = (IXmlConfigLocation)loadedConfig.Location;
            Assert.AreEqual(expectedLocation.Uri, loadedLocation.Uri);
            Assert.AreEqual(expectedLocation.RootElementName, loadedLocation.RootElementName);
            Assert.AreEqual(expectedLocation.RootElementPath, loadedLocation.RootElementPath);


            // Arrange
            var editedConfig = loadedConfig;
            editedConfig.AppVersion = "2.0.0.0";
            
            // Act
            var canSave = sut.CanSave<MyAppConfig>(editedConfig, location: null);
            Assert.IsTrue(canSave);
            sut.Save<MyAppConfig>(editedConfig, location: null);


            // Assert
            var actualConfig = new MyAppConfig();
            LoadFromFile(expectedLocation, sut.FileSystem, actualConfig);

            Assert.IsNotNull(actualConfig);
            Assert.AreEqual(editedConfig.AppName, actualConfig.AppName);
            Assert.AreEqual(editedConfig.AppVersion, actualConfig.AppVersion);
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
