using System.Xml.Linq;

namespace Lux
{
    public static class XmlExtensions
    {
        public static string GetAttributeValue(this XElement element, string attributeName)
        {
            if (element == null)
                return null;
            var attr = element.Attribute(attributeName);
            return attr != null ? attr.Value : null;
        }

        public static XElement SetAttr(this XElement element, string attributeName, object attributeValue)
        {
            if (element == null)
                return null;
            element.SetAttributeValue(attributeName, attributeValue);
            return element;
        }

    }
}
