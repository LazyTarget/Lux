using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Lux.Xml
{
    public class XElementBuilder<TNode> : IXElementBuilder<TNode>
        where TNode : XElement, new()
    {
        private TNode _node;
        private Func<TNode> _factory; 
        
        public XElementBuilder()
        {
            _factory = () => new TNode();
            _node = InstantiateNode();
        }

        public XElementBuilder(TNode node)
        {
            _factory = () => new TNode();

            if (node == null)
                node = InstantiateNode();
            _node = node;
        }
        

        protected virtual TNode InstantiateNode()
        {
            return _factory();
        }

        public virtual IXElementBuilder<TNode> New()
        {
            //_node = InstantiateNode();
            //return this;
            return new XElementBuilder<TNode>(InstantiateNode());
        }

        public virtual IXElementBuilder<TNode> SetTagName(string name)
        {
            _node.Name = name;
            return this;
        }

        public virtual IXElementBuilder<TNode> SetAttribute(string name, object value)
        {
            _node.SetAttributeValue(name, value);
            return this;
        }

        public virtual IXElementBuilder<TNode> SetValue(object value)
        {
            _node.Value = (value ?? "").ToString();
            return this;
        }
        
        public virtual IXElementBuilder<TNode> AppendChild(XNode node)
        {
            _node.Add(node);
            return this;
        }

        public virtual TNode Create()
        {
            return _node;
        }
    }
}
