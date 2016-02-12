using System;
using System.Configuration;
using System.Linq;

namespace Lux.Config
{
    public static class ConfigurationExtensions
    {
        public static ConfigurationSection FindConfigSection(this Configuration configuration, Func<ConfigurationSection, bool> predicate, bool recursive = true)
        {
            var configurationSection = FindConfigSection(configuration.Sections, predicate);
            if (configurationSection == null)
            {
                configurationSection = FindConfigSection(configuration.SectionGroups, predicate, recursive);
            }
            return configurationSection;
        }


        public static ConfigurationSection FindConfigSection(this ConfigurationSectionGroupCollection collection, Func<ConfigurationSection, bool> predicate, bool recursive)
        {
            foreach (var configurationSectionGroup in collection.OfType<ConfigurationSectionGroup>())
            {
                var configurationSection = FindConfigSection(configurationSectionGroup.Sections, predicate);
                if (configurationSection != null)
                    return configurationSection;
                if (recursive)
                {
                    configurationSection = FindConfigSection(configurationSectionGroup.SectionGroups, predicate, recursive);
                    if (configurationSection != null)
                        return configurationSection;
                }
            }
            return null;
        }


        public static ConfigurationSection FindConfigSection(this ConfigurationSectionCollection collection, Func<ConfigurationSection, bool> predicate)
        {
            foreach (var configurationSection in collection.OfType<ConfigurationSection>())
            {
                var cond = predicate(configurationSection);
                if (cond)
                    return configurationSection;
            }
            return null;
        }

    }
}
