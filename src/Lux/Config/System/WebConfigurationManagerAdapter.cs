using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;

namespace Lux.Config
{
    /// <summary>
    /// Class that wraps the functionality within the Web Configuration Manager class
    /// </summary>
    public class WebConfigurationManagerAdapter : IConfigurationManager
    {
        /// <summary>
        /// Gets the System.Configuration.AppSettingsSection data for the current application's default configuration
        /// </summary>
        /// <returns>Returns a System.Collections.Specialized.NameValueCollection object that contains the contents of the System.Configuration.AppSettingsSection object for the current application's default configuration.</returns>
        public NameValueCollection AppSettings => WebConfigurationManager.AppSettings;

        /// <summary>
        /// Gets the System.Configuration.ConnectionStringsSection data for the current application's default configuration.
        /// </summary>
        /// <returns>Returns a System.Configuration.ConnectionStringSettingsCollection object that contains the contents of the System.Configuration.ConnectionStringsSection object for the current application's default configuration.</returns>
        public ConnectionStringSettingsCollection ConnectionStrings => WebConfigurationManager.ConnectionStrings;

        /// <summary>
        /// Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// <typeparam name="T">The type associated with the configuration section</typeparam>
        /// <param name="sectionName">The configuration section path and name.</param>
        /// <returns>The specified System.Configuration.ConfigurationSection object, or null if the section does not exist.</returns>
        public T GetSection<T>(string sectionName)
        {
            var obj = WebConfigurationManager.GetSection(sectionName);
            var result = (T) obj;
            return result;
        }

        public Configuration OpenConfiguration(string path, ConfigurationUserLevel userLevel)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(path);
            return config;
        }
    }
}
