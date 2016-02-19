using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Lux.IO;

namespace Lux.Config
{
    public class LuxConfigurationManager : IConfigurationManager
    {
        private bool _activated;
        private readonly IConfigurationManager _parent;

        public LuxConfigurationManager(IConfigurationManager parent)
        {
            _parent = parent;
            FileSystem = new FileSystem();
        }

        public IFileSystem FileSystem { get; set; }


        public NameValueCollection AppSettings => _parent.AppSettings;

        public ConnectionStringSettingsCollection ConnectionStrings => _parent.ConnectionStrings;

        public T GetSection<T>(string sectionName)
        {
            ActivateIfIsNot();
            var result = _parent.GetSection<T>(sectionName);
            return result;
        }

        public Configuration OpenConfiguration(string path, ConfigurationUserLevel userLevel)
        {
            ActivateIfIsNot();
            //var config = _parent.OpenConfiguration(path, userLevel);

            Configuration config;
            if (path == null)
            {
                config = OpenExeConfiguration(userLevel);
            }
            else
            {
                var fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = path;

                config = OpenMappedExeConfiguration(fileMap, userLevel);
            }
            return config;
        }


        private void ActivateIfIsNot()
        {
            if (!_activated)
            {
                if (!ConfigSystemProxy.IsActivated())
                {
                    ConfigSystemProxy.FileSystem = FileSystem;
                    ConfigSystemProxy.Activate();
                }
                _activated = true;
            }
        }



        internal const string MachineConfigName = "MACHINE";
        internal const string ExeConfigName = "EXE";
        internal const string RoamingUserConfigName = "ROAMING_USER";
        internal const string LocalUserConfigName = "LOCAL_USER";

        internal const string MachineConfigPath = MachineConfigName;
        internal const string ExeConfigPath = MachineConfigPath + "/" + ExeConfigName;
        internal const string RoamingUserConfigPath = ExeConfigPath + "/" + RoamingUserConfigName;
        internal const string LocalUserConfigPath = RoamingUserConfigPath + "/" + LocalUserConfigName;


        //
        // *************************************************
        // ** Static Management Functions to edit config **
        // *************************************************
        //

        //
        // OpenMachineConfiguration
        //
        public static Configuration OpenMachineConfiguration()
        {
            return OpenExeConfigurationImpl(null, true, ConfigurationUserLevel.None, null);
        }

        public static Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap)
        {
            return OpenExeConfigurationImpl(fileMap, true, ConfigurationUserLevel.None, null);
        }

        //
        // OpenExeConfiguration
        //
        public static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
        {
            return OpenExeConfigurationImpl(null, false, userLevel, null);
        }

        public static Configuration OpenExeConfiguration(string exePath)
        {
            return OpenExeConfigurationImpl(null, false, ConfigurationUserLevel.None, exePath);
        }

        public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
        {
            return OpenExeConfigurationImpl(fileMap, false, userLevel, null);
        }

        public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel, bool preLoad)
        {
            return OpenExeConfigurationImpl(fileMap, false, userLevel, null, preLoad);
        }

        private static Configuration OpenExeConfigurationImpl(ConfigurationFileMap fileMap, bool isMachine, ConfigurationUserLevel userLevel, string exePath, bool preLoad = false)
        {
            // exePath must be specified if not running inside ClientConfigurationSystem
            if (!isMachine &&
                 (((fileMap == null) && (exePath == null)) ||
                   ((fileMap != null) && (((ExeConfigurationFileMap)fileMap).ExeConfigFilename == null))
                 )
               )
            {
                //if ( ( s_configSystem != null ) &&
                //     ( s_configSystem.GetType() != typeof( ClientConfigurationSystem ) ) ) {
                //    throw new ArgumentException(SR.GetString(SR.Config_configmanager_open_noexe));
                //}
            }

            Configuration config = OpenExeConfiguration(fileMap, isMachine, userLevel, exePath);
            if (preLoad)
            {
                PreloadConfiguration(config);
            }
            return config;
        }

        /// <summary>
        /// Recursively loads configuration section groups and sections belonging to a configuration object.
        /// </summary>
        private static void PreloadConfiguration(Configuration configuration)
        {
            if (null == configuration)
            {
                return;
            }

            // Preload root-level sections.
            foreach (ConfigurationSection section in configuration.Sections)
            {
            }

            // Recursively preload all section groups and sections.
            foreach (ConfigurationSectionGroup sectionGroup in configuration.SectionGroups)
            {
                PreloadConfigurationSectionGroup(sectionGroup);
            }
        }

        private static void PreloadConfigurationSectionGroup(ConfigurationSectionGroup sectionGroup)
        {
            if (null == sectionGroup)
            {
                return;
            }

            // Preload sections just by iterating.
            foreach (ConfigurationSection section in sectionGroup.Sections)
            {
            }

            // Load child section groups.
            foreach (ConfigurationSectionGroup childSectionGroup in sectionGroup.SectionGroups)
            {
                PreloadConfigurationSectionGroup(childSectionGroup);
            }
        }

        //
        // Create a Configuration object.
        //
        static internal Configuration OpenExeConfiguration(ConfigurationFileMap fileMap, bool isMachine, ConfigurationUserLevel userLevel, string exePath)
        {
            // validate userLevel argument
            switch (userLevel)
            {
                //default:
                //    throw ExceptionUtil.ParameterInvalid("userLevel");

                case ConfigurationUserLevel.None:
                case ConfigurationUserLevel.PerUserRoaming:
                case ConfigurationUserLevel.PerUserRoamingAndLocal:
                    break;
            }

            // validate fileMap arguments
            if (fileMap != null)
            {
                if (String.IsNullOrEmpty(fileMap.MachineConfigFilename))
                {
                    //throw ExceptionUtil.ParameterNullOrEmpty("fileMap.MachineConfigFilename");
                }

                ExeConfigurationFileMap exeFileMap = fileMap as ExeConfigurationFileMap;
                if (exeFileMap != null)
                {
                    switch (userLevel)
                    {
                        case ConfigurationUserLevel.None:
                            if (String.IsNullOrEmpty(exeFileMap.ExeConfigFilename))
                            {
                                //throw ExceptionUtil.ParameterNullOrEmpty("fileMap.ExeConfigFilename");
                            }

                            break;

                        case ConfigurationUserLevel.PerUserRoaming:
                            if (String.IsNullOrEmpty(exeFileMap.RoamingUserConfigFilename))
                            {
                                //throw ExceptionUtil.ParameterNullOrEmpty("fileMap.RoamingUserConfigFilename");
                            }

                            goto case ConfigurationUserLevel.None;

                        case ConfigurationUserLevel.PerUserRoamingAndLocal:
                            if (String.IsNullOrEmpty(exeFileMap.LocalUserConfigFilename))
                            {
                                //throw ExceptionUtil.ParameterNullOrEmpty("fileMap.LocalUserConfigFilename");
                            }

                            goto case ConfigurationUserLevel.PerUserRoaming;
                    }
                }
            }

            string configPath = null;
            if (isMachine)
            {
                configPath = MachineConfigPath;
            }
            else
            {
                switch (userLevel)
                {
                    case ConfigurationUserLevel.None:
                        configPath = ExeConfigPath;
                        break;

                    case ConfigurationUserLevel.PerUserRoaming:
                        configPath = RoamingUserConfigPath;
                        break;

                    case ConfigurationUserLevel.PerUserRoamingAndLocal:
                        configPath = LocalUserConfigPath;
                        break;
                }
            }


            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var configuration = (Configuration) Activator.CreateInstance(typeof (Configuration),
                        flags, null,
                        new object[] {null, typeof (LuxConfigHost), new object[] { fileMap, exePath, configPath} }, null);
            //Configuration configuration = new Configuration(null, typeof(LuxConfigHost), fileMap, exePath, configPath);

            return configuration;
        }

    }
}
