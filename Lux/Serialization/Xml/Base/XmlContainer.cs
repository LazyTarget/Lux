using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlContainer : IXmlNodeContainer
    {
        private readonly IList<IXmlNode> _nodes;


        protected XmlContainer()
        {
            _nodes = new List<IXmlNode>();
        }

        protected XmlContainer(IEnumerable<IXmlNode> nodes)
            : this()
        {
            if (nodes != null)
                _nodes = nodes.ToList();
        }

        
        public IEnumerable<IXmlNode> Nodes()
        {
            return _nodes;
        }

        public void AppendNode(IXmlNode node)
        {
            _nodes.Add(node);
        }

        public void ClearNodes()
        {
            _nodes.Clear();
        }

    }
}
