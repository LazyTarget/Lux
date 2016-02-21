using Lux.Config;
using Lux.Config.Xml;
using Lux.IO;
using NUnit.Framework;

namespace Lux.Tests.Config.XmlConfigManager
{
    [TestFixture]
    public class ConfigDeserializationTests : XmlConfigManagerTestBase
    {
        public ConfigDeserializationTests()
        {
            //FileSystem = new MemoryFileSystem();
            FileSystem = new FileSystem();

            Framework.ConfigurationManager = new LuxConfigurationManager(new ConfigurationManagerAdapter())
            {
                FileSystem = FileSystem,
            };
        }

        protected override void SetUp()
        {
            base.SetUp();
        }

        protected override TestableXmlConfigManager GetSUT()
        {
            return base.GetSUT();
        }


        [TestCase]
        public void Test()
        {
            var xml = @"
<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <configSections>
    <sectionGroup name=""lux"">
      <section name=""MyConfig"" type=""Lux.Tests.Config.XmlConfigManager.XmlConfigManager_IXmlNodeTests+MyConfig, Lux.Tests"" />
      <section name=""MyAppConfig"" type=""Lux.Tests.Config.XmlConfigManager.XmlConfigManager_AppConfigDescriptorTests+MyAppConfig, Lux.Tests"" />
      <section name=""framework"" type=""Lux.FrameworkConfig, Lux"" />
    </sectionGroup>
  </configSections>
  <lux>
    <framework>
      <cultureInfo>sv-SE</cultureInfo>
    </framework>
  </lux>
</configuration>";

            SaveAppConfigFile(xml, FileSystem);
            

            //Framework.LoadFromConfig();

            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<FrameworkConfig>();
            
            var sut = GetSUT();
            var config = sut.Load<FrameworkConfig>(descriptor);

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.CultureInfo);
            Assert.AreEqual("sv-SE", config.CultureInfo.Name);
        }
        
    }
}
