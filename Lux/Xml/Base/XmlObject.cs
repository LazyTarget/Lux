using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
{
    public abstract class XmlObject : XmlNode, IXmlObject
    {
        protected readonly IDictionary<string, object> Data = new Dictionary<string, object>();


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
            var res = GetPropertyNames().Contains(name);
            return res;
        }

        public virtual object GetProperty(string name)
        {
            object res = null;
            var hasProp = HasProperty(name);
            if (hasProp)
            {
                res = Data[name];
            }
            return res;
        }

        public virtual void SetProperty(string name, object value)
        {
            Data[name] = value;
        }

        public virtual void Clear()
        {
            Data.Clear();
        }

        public virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
