using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public abstract class XmlConventionBase : IXmlConfigurator, IXmlExporter
    {
        private IConverter _converter;
        private ITypeInstantiator _typeInstantiator;
        private IXmlInstantiator _xmlInstantiator;

        protected XmlConventionBase()
        {
            _converter = new Converter();
            _typeInstantiator = new TypeInstantiator();
            _xmlInstantiator = new XmlInstantiator();
        }

        public virtual IConverter Converter
        {
            get { return _converter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _converter = value;
            }
        }

        public virtual ITypeInstantiator TypeInstantiator
        {
            get { return _typeInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _typeInstantiator = value;
            }
        }

        public virtual IXmlInstantiator XmlInstantiator
        {
            get { return _xmlInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _xmlInstantiator = value;
            }
        }

        
        public abstract void Configure(IXmlConfigurable configurable, XElement element);
        
        public abstract void Export(IXmlExportable exportable, XElement element);

    }
}
