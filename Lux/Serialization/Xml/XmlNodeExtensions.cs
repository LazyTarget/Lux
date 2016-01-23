using System.Collections.Generic;
using System.Linq;

namespace Lux.Serialization.Xml
{
    public static class XmlNodeExtensions
    {
        public static IEnumerable<TNode> Items<TNode>(this IXmlArray array)
            where TNode : IXmlNode
        {
            return array.Items().OfType<TNode>();
        }
        
        public static object GetPropertyValue(this IXmlObject obj, string name)
        {
            if (obj == null)
                return null;
            var prop = obj.GetProperty(name);
            return prop != null ? prop.Value : null;
        }

    }
}
