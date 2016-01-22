using System;
using Lux.Interfaces;

namespace Lux.Serialization.Xml
{
    public class XmlSettings
    {
        private IConverter _converter;
        private ITypeInstantiator _typeInstantiator;
        private IXmlInstantiator _xmlInstantiator;

        public XmlSettings()
        {
            _typeInstantiator = new TypeInstantiator();
            _converter = new Converter(_typeInstantiator);
            _xmlInstantiator = new XmlInstantiator(this);
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

    }
}
