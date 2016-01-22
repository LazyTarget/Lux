using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Xml
{
    public static class XmlNodeExtensions
    {
        public static IEnumerable<TNode> Nodes<TNode>(this IXmlArray array)
            where TNode : IXmlNode
        {
            return array.Nodes().OfType<TNode>();
        }

    }
}
