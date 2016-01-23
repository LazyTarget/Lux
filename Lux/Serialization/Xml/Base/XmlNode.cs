using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlNode : IXmlNode
    {
        protected XmlNode()
            : this(XmlPattern.Instance)
        {

        }

        protected XmlNode(IXmlPattern pattern)
            : this(pattern, null)
        {
            
        }

        protected XmlNode(IXmlPattern pattern, IXmlNode parentNode)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));
            Pattern = pattern;
            ParentNode = parentNode;
        }

        public IXmlPattern Pattern { get; }

        public IXmlNode ParentNode { get; }


        public virtual void Configure(XElement element)
        {
            Pattern.Configure(this, element);
        }

        public virtual void Export(XElement element)
        {
            Pattern.Export(this, element);
        }

        public IEnumerable<IXmlNode> Nodes()
        {
            throw new NotImplementedException();
        }

        public virtual void AppendNode(IXmlNode node)
        {
            
        }

        public virtual void ClearNodes()
        {
            
        }
    }
}
