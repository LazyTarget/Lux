using System;
using System.Configuration;

namespace Lux.Config
{
    public class AppXmlConfigManager : XmlConfigManager
    {
        protected override IConfigLocation GetLocationOrDefault(IConfigLocation location)
        {
            if (location == null)
            {
                var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                try
                {
                    configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
                }
                catch (Exception ex)
                {
                    
                }

                var configUri = new Uri(configPath);

                location = new XmlConfigLocation
                {
                    Uri = configUri,
                    RootElementName = "configuration",
                    RootElementExpression = "configuration/lux",
                };
            }

            location = base.GetLocationOrDefault(location);
            return location;
        }
    }
}
