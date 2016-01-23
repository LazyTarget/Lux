using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlObject : XmlNode, IXmlObject
    {
        //protected readonly IDictionary<string, object> Data = new Dictionary<string, object>();
        protected readonly IDictionary<string, IProperty> Data = new Dictionary<string, IProperty>();


        protected XmlObject()
            : this(XmlPattern.Instance)
        {

        }

        protected XmlObject(IXmlPattern pattern)
            : this(pattern, null)
        {

        }

        protected XmlObject(IXmlPattern pattern, IXmlNode parentNode)
            : base(pattern, parentNode)
        {

        }


        public IEnumerable<string> GetPropertyNames()
        {
            return GetProperties().Where(x => x != null).Select(x => x.Name);
        }

        public virtual IEnumerable<IProperty> GetProperties()
        {
            return Data.Values.Where(x => x != null);
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
                res = Data[name];
            }
            return res;
        }

        protected virtual void SetProperty(IProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrEmpty(property.Name))
                throw new ArgumentNullException(nameof(property.Name));

            Data[property.Name] = property;

            //SetPropertyValue(property.Name, property.Value);
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
                throw new KeyNotFoundException($"Property '{name}' not found");
            }
        }

        public virtual void ClearProperties()
        {
            Data.Clear();
        }
    }
}
