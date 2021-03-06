﻿using System;
using System.Globalization;
using System.Xml.Linq;
using Lux.Config;
using Lux.Config.Xml;
using Lux.Dependency;
using Lux.Diagnostics;
using Lux.Interfaces;
using Lux.Unittest;

namespace Lux
{
    public static class Framework
    {
        private static IConfigManager _configManager;
        private static IConfigurationManager _configurationManager;
        private static ILogFactory _logFactory;
        private static ITypeInstantiator _typeInstantiator;
        private static IAsserter _asserter;
        private static IDependencyContainer _dependencyContainer;

        static Framework()
        {
            //CultureInfo = CultureInfo.CurrentCulture;
            CultureInfo = null;

            ConfigurationManager = new ConfigurationManagerAdapter();
            //ConfigurationManager = new LuxConfigurationManager(new ConfigurationManagerAdapter());

            ConfigManager = new XmlConfigManager();
            TypeInstantiator = new TypeInstantiator();
            Asserter = new EmptyAsserter();
            LogFactory = new NullObjectLogFactory();

#if DEBUG
            LogFactory = new DebugLogFactory();
#endif
        }


        public static CultureInfo CultureInfo { get; set; }
        
        public static ITypeInstantiator TypeInstantiator
        {
            get { return _typeInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _typeInstantiator = value;
            }
        }

        public static ILogFactory LogFactory
        {
            get { return _logFactory; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _logFactory = value;
            }
        }

        public static IConfigManager ConfigManager
        {
            get { return _configManager; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _configManager = value;
            }
        }

        public static IConfigurationManager ConfigurationManager
        {
            get { return _configurationManager; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _configurationManager = value;
            }
        }

        public static IAsserter Asserter
        {
            get { return _asserter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _asserter = value;
            }
        }

        public static IDependencyContainer DependencyContainer
        {
            get { return _dependencyContainer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _dependencyContainer = value;
            }
        }



        public static void LoadFromConfig()
        {
            var section = ConfigurationManager.GetSection<object>("lux.framework");


            var element = section as XElement;

            var config = new FrameworkConfig();
            config.Configure(element);
            config.Apply();
        }
        
    }
}
