using System;
using System.Xml.Linq;
using Lux.Data;
using Lux.IO;
using Lux.Serialization.Xml;

namespace Lux.Config.Xml
{
    public class XmlConfigManager : IConfigManager
    {
        public XmlConfigManager()
        {
            DefaultDescriptorFactory = new AppConfigDescriptorFactory
            {
                ConfigurationManager = Framework.ConfigurationManager,
            };
        }

        public IConfigDescriptorFactory DefaultDescriptorFactory { get; set; }

        public bool SaveAndReplace { get; set; }



        public virtual IConfigDescriptor GetDefaultDescriptor<TConfig>()
            where TConfig : IConfig
        {
            IConfigDescriptor descriptor = null;
            if (DefaultDescriptorFactory != null)
            {
                descriptor = DefaultDescriptorFactory.CreateDescriptor<TConfig>();
            }
            else
            {
                descriptor = new XmlConfigDescriptor();
            }
            return descriptor;
        }

        public bool CanLoad<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig
        {
            if (descriptor == null)
                descriptor = GetDefaultDescriptor<TConfig>();
            ValidateDescriptor(descriptor);

            var configXmlDataStore = descriptor.DataStore as ConfigXmlDataStore;
            if (configXmlDataStore != null)
            {
                if (configXmlDataStore.Uri == null)
                    return false;
            }

            if (typeof(TConfig).IsAssignableFrom(typeof(IXmlConfigurable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public TConfig Load<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig
        {
            if (descriptor == null)
                descriptor = GetDefaultDescriptor<TConfig>();
            ValidateDescriptor(descriptor);

            // Load
            var obj = descriptor.DataStore.Load(descriptor);
            var document = (XDocument) obj;
            
            // Parse
            var config = descriptor.Parser.Parse<TConfig>(descriptor, document);
            config.Descriptor = descriptor;
            return config;
        }


        public bool CanSave<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig
        {
            var descriptor = config.Descriptor;
            if (descriptor == null)
                descriptor = GetDefaultDescriptor<TConfig>();
            ValidateDescriptor(descriptor);
            
            var configXmlDataStore = dataStore as ConfigXmlDataStore;
            if (configXmlDataStore != null)
            {
                if (configXmlDataStore.Uri == null)
                    return false;
                if (!configXmlDataStore.Uri.IsFile)
                    return false;
            }

            if (typeof(TConfig).IsAssignableFrom(typeof(IXmlExportable)))
                return false;   // todo: extend, use a XmlSerializer?

            return true;
        }


        public virtual object Save<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig
        {
            var descriptor = config.Descriptor;
            if (descriptor == null)
            {
                descriptor = GetDefaultDescriptor<TConfig>();
                config.Descriptor = descriptor;
            }
            ValidateDescriptor(descriptor);

            // Load
            object obj;
            XDocument xdoc = null;
            if (SaveAndReplace)
            {
                // Should save as new
                //xdoc = new XDocument();
            }
            else
            {
                if (descriptor.DataStore != null)
                {
                    obj = descriptor.DataStore.Load(descriptor);
                    //xdoc = (XDocument) obj;
                    xdoc = obj as XDocument;
                }
            }

            // Update
            obj = descriptor.Parser.Export<TConfig>(config, xdoc);
            xdoc = (XDocument) obj;

            // Save
            var targetDataStore = dataStore ?? descriptor.DataStore;
            //var targetDataStore = dataStore;
            if (targetDataStore == null)
                throw new InvalidOperationException("Invalid save target");
            var res = targetDataStore.Save(descriptor, xdoc);

            return res;
        }


        protected virtual bool ValidateDescriptor(IConfigDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            //if (!(descriptor is XmlConfigDescriptor))
            //    throw new NotSupportedException($"ConfigSource of type '{descriptor.GetType()}' is not supported");

            return true;
        }

        
    }
}