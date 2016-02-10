using System;
using Lux.Config;
using Lux.Interfaces;

namespace Lux
{
    public static class Framework
    {
        private static IConfigManager _configManager;
        private static ITypeInstantiator _typeInstantiator;

        static Framework()
        {
            TypeInstantiator = new TypeInstantiator();
            ConfigManager = new XmlConfigManager();
        }


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
    }
}
