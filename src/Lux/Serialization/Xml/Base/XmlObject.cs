using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class XmlObject : XmlContainer, IXmlObject
    {
        public XElement Element { get; internal set; }
        public IXmlPattern Pattern { get; internal set; }
        private readonly IDictionary<string, IProperty> _properties;

        public XmlObject()
            : this("object")
        {
        }

        public XmlObject(string tagName)
            : this(new XElement(tagName))
        {
        }

        protected internal XmlObject(XElement element)
            : this(element, XmlPattern.Default)
        {
        }

        protected internal XmlObject(XElement element, IXmlPattern pattern)
        {
            //if (element == null)
            //    throw new ArgumentNullException(nameof(element));
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            _properties = new Dictionary<string, IProperty>();
            Pattern = pattern;
            Element = element;
        }


        public string TagName { get; set; }


        public IEnumerable<string> GetPropertyNames()
        {
            return GetProperties().Where(x => x != null).Select(x => x.Name);
        }

        public virtual IEnumerable<IProperty> GetProperties()
        {
            return _properties.Values.Where(x => x != null);
        }

        public virtual bool HasProperty(string name)
        {
            //var res = Data.ContainsKey(name);
            //var res = GetPropertyNames().Contains(name);
            var res = GetPropertyNames().Contains(name);
            return res;
        }

        public virtual IProperty GetProperty(string name)
        {
            IProperty res = null;
            var hasProp = HasProperty(name);
            if (hasProp)
            {
                res = _properties[name];
            }
            return res;
        }

        protected virtual void DefineProperty(IProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrEmpty(property.Name))
                throw new ArgumentNullException(nameof(property.Name));

            if (_properties.ContainsKey(property.Name))
                throw new InvalidOperationException($"Property '{property.Name}' already defined");

            _properties[property.Name] = property;

            SetPropertyValue(property.Name, property.Value);
        }

        public virtual void SetPropertyValue(string name, object value)
        {
            var property = GetProperty(name);
            if (property != null)
            {
                property.SetValue(value);
            }
            else
            {
                //throw new KeyNotFoundException($"Property '{name}' not found");

                var type = value?.GetType();
                var prop = new Property(name, type, value);
                DefineProperty(prop);
            }
        }

        public virtual void ClearProperties()
        {
            _properties.Clear();
        }


        public virtual void Configure(XElement element)
        {
            //Pattern.Configure(this, element);
        }

        public virtual void Export(XElement element)
        {
            //Pattern.Export(this, element);
        }

    }
}
