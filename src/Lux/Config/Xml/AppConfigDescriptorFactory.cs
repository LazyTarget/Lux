using System;
using System.Configuration;
using Lux.Data;
using Lux.IO;

namespace Lux.Config.Xml
{
    public class AppConfigDescriptorFactory : IConfigDescriptorFactory
    {
        public AppConfigDescriptorFactory()
        {
            FileSystem = new FileSystem();
            UserLevel = ConfigurationUserLevel.None;
        }

        public IFileSystem FileSystem { get; set; }

        public ConfigurationUserLevel UserLevel { get; set; }

        
        public virtual IConfigDescriptor CreateDescriptor(Type configType)
        {
            string rootElementPath = null;
            var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(UserLevel);
                configPath = config.FilePath;
                
                var configSection = config.FindConfigSection(section =>
                {
                    var assName = configType.Assembly.GetName();
                    var type = $"{configType.FullName}, {assName.Name}";
                    if (section.SectionInformation.Type == type)
                        return true;
                    if (section.SectionInformation.Type == configType.FullName)
                        return true;
                    return false;
                });
                if (configSection != null)
                {
                    rootElementPath = "configuration/" + configSection.SectionInformation.SectionName;
                }
            }
            catch (Exception ex)
            {

            }
            
            var configUri = new Uri(configPath);
            IDataStore<IConfigDescriptor> dataStore = new ConfigXmlDataStore
            {
                Uri = configUri,
            };

            IConfigDescriptor location = new XmlConfigDescriptor
            {
                Uri = configUri,
                RootElementPath = rootElementPath,
                DataStore = dataStore,
            };
            return location;
        }


        public IConfigDescriptor CreateDescriptor<TConfig>() 
            where TConfig : IConfig
        {
            var type = typeof (TConfig);
            var result = CreateDescriptor(type);
            return result;
        }

    }
}
