using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Lux.Xml
{
    public class XElementBuilder : IXElementBuilder
    {
        private XElement _node;
        private Func<XElement> _factory;

        public XElementBuilder()
            : this("element")
        {
        }

        public XElementBuilder(string tagName)
        {
            _factory = () => new XElement(tagName);
            _node = InstantiateNode();
        }

        public XElementBuilder(XElement node)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            node.Save(tw);
            var xml = sb.ToString();

            _factory = () => XElement.Parse(xml);
            _node = node;
        }


        protected virtual XElement InstantiateNode()
        {
            return _factory();
        }

        public virtual IXElementBuilder New()
        {
            //_node = InstantiateNode();
            //return this;
            return new XElementBuilder(InstantiateNode());
        }

        public virtual IXElementBuilder SetTagName(string name)
        {
            _node.Name = name;
            return this;
        }

        public virtual IXElementBuilder SetAttribute(string name, object value)
        {
            _node.SetAttributeValue(name, value);
            return this;
        }

        public virtual IXElementBuilder SetValue(object value)
        {
            _node.Value = (value ?? "").ToString();
            return this;
        }
        
        public virtual IXElementBuilder AppendChild(XNode node)
        {
            _node.Add(node);
            return this;
        }

        public virtual XElement Create()
        {
            return _node;
        }
    }
}
