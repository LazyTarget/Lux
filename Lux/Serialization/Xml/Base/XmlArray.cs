using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlArray<T> : XmlNode, IXmlArray
        where T : IXmlNode
    {
        protected readonly IList<T> Data = new List<T>();


        protected XmlArray()
            : this(XmlPattern.Instance)
        {

        }

        protected XmlArray(IXmlPattern pattern)
            : this(pattern, null)
        {

        }

        protected XmlArray(IXmlPattern pattern, IXmlNode parentNode)
            : base(pattern, parentNode)
        {
            
        }


        public virtual IEnumerable<IXmlNode> Items()
        {
            return Data.Cast<IXmlNode>().AsEnumerable();
        }
        
        IEnumerable<INode> IArray.Items()
        {
            return Items();
        }

        public virtual void AddItem(object item)
        {
            var node = (IXmlNode) item;
            AddItem(node);
        }

        public virtual void AddItem(IXmlNode item)
        {
            var obj = (T) item;
            Data.Add(obj);
        }

        public virtual void ClearItems()
        {
            Data.Clear();
        }
        
    }
}
