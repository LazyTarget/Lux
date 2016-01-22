using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lux.Serialization;

namespace Lux.Xml
{
    public abstract class XmlObject : XmlNode, IXmlObject
    {
        //protected readonly IDictionary<string, object> Data = new Dictionary<string, object>();
        protected readonly IDictionary<string, IProperty> Data = new Dictionary<string, IProperty>();


        protected XmlObject()
            : this(XmlPattern.Instance)
        {

        }

        protected XmlObject(XmlPattern pattern)
            : this(pattern, null)
        {

        }

        protected XmlObject(XmlPattern pattern, IXmlNode parentNode)
            : base(pattern, parentNode)
        {

        }


        public virtual IEnumerable<string> GetPropertyNames()
        {
            return Data.Keys;
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

        protected virtual void SetProperty(string name, IProperty property)
        {
            Data[name] = property;
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

        public virtual void Clear()
        {
            Data.Clear();
        }

        public virtual IEnumerator<IProperty> GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
