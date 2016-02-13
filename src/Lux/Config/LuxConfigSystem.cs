using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Configuration.Internal;
using System.Reflection;
using Lux.Extensions;

namespace Lux.Config
{
    public sealed class LuxConfigSystem : IInternalConfigSystem
    {
        private static IInternalConfigSystem _clientConfigSystem;

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

            if (fiInit != null && fiSystem != null && null != fiStateValues)
            {
                fiInit.SetValue(null, fiStateValues[1].GetValue(null));
                fiSystem.SetValue(null, null);
            }

            LuxConfigSystem confSys = new LuxConfigSystem();
            Type configFactoryType = Type.GetType("System.Configuration.Internal.InternalConfigSettingsFactory, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            IInternalConfigSettingsFactory configSettingsFactory = (IInternalConfigSettingsFactory)Activator.CreateInstance(configFactoryType, true);
            configSettingsFactory.SetConfigurationSystem(confSys, false);

            Type clientConfigSystemType = Type.GetType("System.Configuration.ClientConfigurationSystem, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
            _clientConfigSystem = (IInternalConfigSystem)Activator.CreateInstance(clientConfigSystemType, true);
        }


        private object _appSettings;
        private object _connectionStrings;

        public LuxConfigSystem()
        {
            
        }


        #region IInternalConfigSystem Members

        public object GetSection(string configKey)
        {
            // get the section from the default location (web.config or app.config)
            object section = _clientConfigSystem.GetSection(configKey);

            switch (configKey)
            {
                case "appSettings":
                    if (this._appSettings != null)
                    {
                        return this._appSettings;
                    }

                    if (section is NameValueCollection)
                    {
                        // create a new collection because the underlying collection is read-only
                        var cfg = new NameValueCollection((NameValueCollection)section);

                        // merge the settings from core with the local appsettings
                        this._appSettings = cfg.Merge(System.Configuration.ConfigurationManager.AppSettings);
                        section = this._appSettings;
                    }
                    break;

                case "connectionStrings":
                    if (this._connectionStrings != null)
                    {
                        return this._connectionStrings;
                    }

                    // create a new collection because the underlying collection is read-only
                    var cssc = new ConnectionStringSettingsCollection();

                    // copy the existing connection strings into the new collection
                    foreach (ConnectionStringSettings connectionStringSetting in ((ConnectionStringsSection)section).ConnectionStrings)
                    {
                        cssc.Add(connectionStringSetting);
                    }

                    // merge the settings from core with the local connectionStrings
                    cssc = cssc.Merge(System.Configuration.ConfigurationManager.ConnectionStrings);

                    // Cannot simply return our ConnectionStringSettingsCollection as the calling routine expects a ConnectionStringsSection result
                    ConnectionStringsSection connectionStringsSection = new ConnectionStringsSection();

                    // Add our merged connection strings to the new ConnectionStringsSection
                    foreach (ConnectionStringSettings connectionStringSetting in cssc)
                    {
                        connectionStringsSection.ConnectionStrings.Add(connectionStringSetting);
                    }

                    this._connectionStrings = connectionStringsSection;
                    section = this._connectionStrings;
                    break;
            }

            return section;
        }

        public void RefreshConfig(string sectionName)
        {
            if (sectionName == "appSettings")
            {
                this._appSettings = null;
            }

            if (sectionName == "connectionStrings")
            {
                this._connectionStrings = null;
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
