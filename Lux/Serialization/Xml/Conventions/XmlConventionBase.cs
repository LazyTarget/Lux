using System;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Serialization.Xml
{
    public abstract class XmlConventionBase : IXmlConfigurator, IXmlExporter
    {
        private XmlSettings _xmlSettings;

        protected XmlConventionBase()
        {
            _xmlSettings = new XmlSettings();
        }

        public virtual XmlSettings XmlSettings
        {
            get { return _xmlSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _xmlSettings = value;
            }
        }

        protected IConverter Converter                  => XmlSettings.Converter;
        protected ITypeInstantiator TypeInstantiator    => XmlSettings.TypeInstantiator;
        protected IXmlInstantiator XmlInstantiator      => XmlSettings.XmlInstantiator;


        public abstract void Configure(IXmlConfigurable configurable, XElement element);
        
        public abstract void Export(IXmlExportable exportable, XElement element);

    }
}
