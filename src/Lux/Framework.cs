using System;
using Lux.Config;
using Lux.Interfaces;
using Lux.Unittest;

namespace Lux
{
    public static class Framework
    {
        private static IConfigManager _configManager;
        private static ITypeInstantiator _typeInstantiator;
        private static IAsserter _asserter;

        static Framework()
        {
            TypeInstantiator = new TypeInstantiator();
            ConfigManager = new XmlConfigManager();
            Asserter = new EmptyAsserter();
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

    }
}
