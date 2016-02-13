using System;
using System.Xml.Linq;
using Lux.Data;
using Lux.Extensions;
using Lux.Interfaces;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Config.Xml
{
    public class XmlConfigParser : IConfigParser
    {
        public XmlConfigParser()
        {
            TypeInstantiator = Framework.TypeInstantiator;
        }

        public ITypeInstantiator TypeInstantiator { get; set; }


        public TConfig Parse<TConfig>(IConfigDescriptor descriptor, object data) 
            where TConfig : IConfig
        {
            var dataStore = descriptor.DataStore as IDataStore<IConfigDescriptor, XDocument>;
            if (dataStore == null)
                throw new ArgumentException("Invalid datastore", nameof(descriptor));

            var document = ParseInputAsDocument(data);
            var config = ParseFromXDocument<TConfig>(document, descriptor);
            return config;
        }

        public object Export<TConfig>(TConfig config, object data) 
            where TConfig : IConfig
        {
            var document = ParseInputAsDocument(data);
            document = ExportToXDocument(document, config);
            return document;
        }


        protected virtual XDocument ParseInputAsDocument(object data)
        {
            XDocument document;
            if (data is string)
            {
                var xml = (data ?? "").ToString();
                document = XDocument.Parse(xml);
            }
            else if (data is XDocument)
            {
                document = (XDocument) data;
            }
            else if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else
            {
                throw new NotSupportedException($"The type '{data.GetType().Name}' is not supported for parsing");
            }
            return document;
        }


        protected virtual TConfig ParseFromXDocument<TConfig>(XDocument document, IConfigDescriptor descriptor)
            where TConfig : IConfig
        {
            if (document == null)
                document = new XDocument();

            var path = descriptor?.Properties.GetOrDefault(nameof(XmlConfigDescriptor.RootElementPath)).CastTo<string>();
            var rootElement = document.GetElementByPath(path);

            //TConfig config = default(TConfig);
            var config = TypeInstantiator.Instantiate<TConfig>();
            if (rootElement != null)
            {
                config.Descriptor = descriptor;
                var xmlConfigurable = config as IXmlConfigurable;
                if (xmlConfigurable != null)
                {
                    xmlConfigurable.Configure(rootElement);
                }
                else
                {
                    // todo: Use XmlSerializer?
                }
            }
            return config;
        }
        

        protected virtual XDocument ExportToXDocument(XDocument document, IConfig config)
        {
            if (document == null)
                document = new XDocument();
            
            var path = config?.Descriptor?.Properties.GetOrDefault(nameof(XmlConfigDescriptor.RootElementPath)).CastTo<string>();
            var rootElement = document.GetOrCreateElementAtPath(path);
            if (rootElement != null)
            {
                var xmlExportable = config as IXmlExportable;
                if (xmlExportable != null)
                {
                    xmlExportable.Export(rootElement);
                }
                else
                {
                    // todo: Use XmlSerializer?
                }
            }
            else
            {
                
            }
            return document;
        }

    }
}