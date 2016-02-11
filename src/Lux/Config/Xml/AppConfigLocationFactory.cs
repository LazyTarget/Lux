using System;
using System.Configuration;
using System.Linq;

namespace Lux.Config
{
    public class AppConfigLocationFactory : IConfigLocationFactory
    {
        public virtual IConfigLocation CreateLocation<TConfig>()
            where TConfig : IConfig
        {
            string rootElementPath = null;

            var configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                configPath = config.FilePath;
            }
            catch (Exception ex)
            {

            }
            
            var configUri = new Uri(configPath);
            var location = new XmlConfigLocation
            {
                Uri = configUri,
                RootElementPath = rootElementPath,
            };
            return location;
        }
    }
}
