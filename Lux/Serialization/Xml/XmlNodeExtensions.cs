using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public static class XmlNodeExtensions
    {
        [Obsolete("Remove?")]
        public static IEnumerable<TNode> Items<TNode>(this IXmlArray array)
            where TNode : IXmlNode
        {
            return array.Items().OfType<TNode>();
        }

    }
}
