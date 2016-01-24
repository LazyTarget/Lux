using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class XmlList<T> : XmlObject, IXmlArray<T>, IArray<T>
        where T : IXmlNode
    {
        protected readonly IList<T> Data = new List<T>();


        public XmlList()
            : this("list")
        {
        }

        public XmlList(string tagName)
            : this(new XElement(tagName))
        {
        }

        public XmlList(XElement element)
            : this(element, XmlPattern.Default)
        {
        }

        public XmlList(XElement element, IXmlPattern pattern)
            : base(element, pattern)
        {
            
        }



        public virtual void AddItem(object item)
        {
            var node = (T) item;
            AddItem(node);
        }

        public virtual void AddItem(T item)
        {
            Data.Add(item);
        }
        
        public virtual void ClearItems()
        {
            Data.Clear();
        }
        

        IEnumerable<object> IArray.Items()
        {
            return Data.Cast<object>().AsEnumerable();
        }

        IEnumerable<T> IArray<T>.Items()
        {
            return Data.AsEnumerable();
        }
    }
}
