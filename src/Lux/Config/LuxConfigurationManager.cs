using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Lux.Config
{
    public class LuxConfigurationManager : IConfigurationManager
    {
        private bool _activated;
        private readonly IConfigurationManager _parent;

        public LuxConfigurationManager(IConfigurationManager parent)
        {
            _parent = parent;
        }


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
            var config = _parent.OpenConfiguration(path, userLevel);
            return config;
        }


        private void ActivateIfIsNot()
        {
            if (!_activated)
            {
                LuxConfigSystem.Activate();
                _activated = true;
            }
        }
    }
}
