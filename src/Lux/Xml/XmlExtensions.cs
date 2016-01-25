using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
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



        public static XElement GetOrCreateElement(this XContainer container, string elementName)
        {
            var elem = GetOrCreateElement(container, elementName, null);
            return elem;
        }

        public static XElement GetOrCreateElement(this XContainer container, string elementName, Func<XElement, bool> predicate)
        {
            IEnumerable<XElement> elems;
            if (predicate != null)
                elems = container.Elements(elementName).Where(predicate);
            else
                elems = container.Elements(elementName);
            var elem = elems.FirstOrDefault();
            if (elem == null)
            {
                elem = new XElement(elementName);
                container.Add(elem);
            }
            return elem;
        }

    }
}
