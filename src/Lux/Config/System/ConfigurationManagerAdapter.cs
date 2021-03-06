﻿using System.Collections.Specialized;
using System.Configuration;

namespace Lux.Config
{
    /// <summary>
    /// Class that wraps the functionality within the standard Configuration Manager class
    /// </summary>
    public class ConfigurationManagerAdapter : IConfigurationManager
    {
        /// <summary>
        /// Gets the System.Configuration.AppSettingsSection data for the current application's default configuration
        /// </summary>
        /// <returns>Returns a System.Collections.Specialized.NameValueCollection object that contains the contents of the System.Configuration.AppSettingsSection object for the current application's default configuration.</returns>
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        /// <summary>
        /// Gets the System.Configuration.ConnectionStringsSection data for the current application's default configuration.
        /// </summary>
        /// <returns>Returns a System.Configuration.ConnectionStringSettingsCollection object that contains the contents of the System.Configuration.ConnectionStringsSection object for the current application's default configuration.</returns>
        public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;

        /// <summary>
        /// Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// <typeparam name="T">The type associated with the configuration section</typeparam>
        /// <param name="sectionName">The configuration section path and name.</param>
        /// <returns>The specified System.Configuration.ConfigurationSection object, or null if the section does not exist.</returns>
        public T GetSection<T>(string sectionName)
        {
            var obj = ConfigurationManager.GetSection(sectionName);
            var result = (T) obj;
            return result;
        }

        public Configuration OpenConfiguration(string path, ConfigurationUserLevel userLevel)
        {
            Configuration config;
            if (path == null)
            {
                config = ConfigurationManager.OpenExeConfiguration(userLevel);
                return config;
            }
            
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = path;

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, userLevel);
            return config;
        }
    }
}
