using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlArray<T> : XmlNode, IXmlArray, IEnumerable<T>
        where T : IXmlNode
    {
        protected readonly IList<T> Data = new List<T>();


        protected XmlArray()
            : this(XmlPattern.Instance)
        {

        }

        protected XmlArray(XmlPattern pattern)
            : this(pattern, null)
        {

        }

        protected XmlArray(XmlPattern pattern, IXmlNode parentNode)
            : base(pattern, parentNode)
        {
            
        }


        public virtual IEnumerable<IXmlNode> Nodes()
        {
            return Data.Cast<IXmlNode>().AsEnumerable();
        }


        public virtual void AddItem(IXmlNode node)
        {
            var obj = (T) node;
            Data.Add(obj);
        }

        public virtual void Clear()
        {
            Data.Clear();
        }

        
        public virtual IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
