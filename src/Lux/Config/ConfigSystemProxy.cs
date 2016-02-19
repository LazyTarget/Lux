using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Configuration.Internal;
using System.IO;
using System.Reflection;
using Lux.Extensions;
using Lux.IO;

namespace Lux.Config
{
    public sealed class ConfigSystemProxy : IInternalConfigSystem
    {
        internal static IFileSystem _fileSystem;
        private static ConfigSystemProxy _activatedConfigSystem;
        internal static IInternalConfigSystem _clientConfigSystem;
        internal static IInternalConfigClientHost _internalConfigClientHost;
        internal static IInternalConfigHost _internalConfigHost;


        public static bool IsActivated()
        {
            var res = _activatedConfigSystem != null;
            return res;
        }

        /// <summary>
        /// Re-initializes the ConfigurationManager, allowing us to merge in the settings from Core.Config
        /// </summary>
        public static void Activate()
        {
            FieldInfo[] fiStateValues = null;
            Type tInitState = typeof(System.Configuration.ConfigurationManager).GetNestedType("InitState", BindingFlags.NonPublic);

            if (tInitState != null)
            {
                fiStateValues = tInitState.GetFields();
            }

            FieldInfo fiInit = typeof(System.Configuration.ConfigurationManager).GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo fiSystem = typeof(System.Configuration.ConfigurationManager).GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static);

            if (fiInit != null && fiSystem != null && fiStateValues != null)
            {
                var state = fiStateValues[1].GetValue(null);
                fiInit.SetValue(null, state);
                fiSystem.SetValue(null, null);
            }

            ConfigSystemProxy confSys = new ConfigSystemProxy();
            Type configFactoryType = Type.GetType("System.Configuration.Internal.InternalConfigSettingsFactory, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            IInternalConfigSettingsFactory configSettingsFactory = (IInternalConfigSettingsFactory)Activator.CreateInstance(configFactoryType, true);
            configSettingsFactory.SetConfigurationSystem(confSys, false);

            Type clientConfigSystemType = Type.GetType("System.Configuration.ClientConfigurationSystem, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            _clientConfigSystem = (IInternalConfigSystem)Activator.CreateInstance(clientConfigSystemType, true);


            // Fetch configHost
            FieldInfo fiClientHost = clientConfigSystemType.GetField("_configHost", BindingFlags.NonPublic | BindingFlags.Instance);
            _internalConfigClientHost = (IInternalConfigClientHost) fiClientHost?.GetValue(_clientConfigSystem);
            
            FieldInfo fiConfigHost = typeof(DelegatingConfigHost).GetField("_host", BindingFlags.NonPublic | BindingFlags.Instance);
            _internalConfigHost = (IInternalConfigHost)fiConfigHost?.GetValue(_internalConfigClientHost);

            // Set custom configHost
            var configHost = new LuxConfigHost(_internalConfigHost);
            if (_fileSystem != null)
                configHost.FileSystem = _fileSystem;
            fiConfigHost.SetValue(_internalConfigClientHost, configHost);
        }
        

        
        private object _appSettings;
        private object _connectionStrings;

        private ConfigSystemProxy()
        {
            
        }


        #region IInternalConfigSystem Members

        public object GetSection(string configKey)
        {
            if (configKey == "appSettings")
            {
                if (_appSettings != null)
                    return _appSettings;
            }
            else if (configKey == "connectionStrings")
            {
                if (_connectionStrings != null)
                    return _connectionStrings;
            }


            // get the section from the default location (web.config or app.config)
            object section = _clientConfigSystem.GetSection(configKey);
            

            switch (configKey)
            {
                case "appSettings":
                    if (section is NameValueCollection)
                    {
                        // create a new collection because the underlying collection is read-only
                        var cfg = new NameValueCollection((NameValueCollection)section);
                        _appSettings = cfg;
                        
                        // merge the settings from core with the local appsettings
                        _appSettings = cfg.Merge(System.Configuration.ConfigurationManager.AppSettings);
                        section = _appSettings;
                    }
                    break;

                case "connectionStrings":
                    // Cannot simply return our ConnectionStringSettingsCollection as the calling routine expects a ConnectionStringsSection result
                    ConnectionStringsSection connectionStringsSection = new ConnectionStringsSection();
                    _connectionStrings = connectionStringsSection;

                    // create a new collection because the underlying collection is read-only
                    var cssc = new ConnectionStringSettingsCollection();

                    // copy the existing connection strings into the new collection
                    foreach (ConnectionStringSettings connectionStringSetting in ((ConnectionStringsSection)section).ConnectionStrings)
                    {
                        cssc.Add(connectionStringSetting);
                    }

                    //// merge the settings from core with the local connectionStrings
                    cssc = cssc.Merge(System.Configuration.ConfigurationManager.ConnectionStrings);
                    
                    // Add our merged connection strings to the new ConnectionStringsSection
                    foreach (ConnectionStringSettings connectionStringSetting in cssc)
                    {
                        connectionStringsSection.ConnectionStrings.Add(connectionStringSetting);
                    }

                    _connectionStrings = connectionStringsSection;
                    section = _connectionStrings;
                    break;
            }

            return section;
        }

        public void RefreshConfig(string sectionName)
        {
            if (sectionName == "appSettings")
            {
                _appSettings = null;
            }

            if (sectionName == "connectionStrings")
            {
                _connectionStrings = null;
            }

            _clientConfigSystem.RefreshConfig(sectionName);
        }

        public bool SupportsUserConfig
        {
            get { return _clientConfigSystem.SupportsUserConfig; }
        }

        #endregion

    }
}
