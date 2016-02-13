using System;
using System.Configuration;

namespace Lux.Config.Xml
{
    public class AppConfigLocationFactory : IConfigLocationFactory
    {
        public AppConfigLocationFactory()
        {
            UserLevel = ConfigurationUserLevel.None;
        }

        public ConfigurationUserLevel UserLevel { get; set; }


        public virtual IConfigLocation CreateLocation<TConfig>()
            where TConfig : IConfig
        {
            string rootElementPath = null;
            var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                //var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(UserLevel);
                var config = Framework.ConfigurationManager.OpenConfiguration(configPath, UserLevel);
                configPath = config.FilePath;

                var configType = typeof (TConfig);
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
            IConfigLocation location = new XmlConfigLocation
            {
                Uri = configUri,
                RootElementPath = rootElementPath,
            };
            return location;
        }
    }
}
