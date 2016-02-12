using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

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




        
        public static XElement GetElementByPath(this XContainer node, string path)
        {
            XElement rootElement = null;
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var expr = XPathExpression.Compile(path);
                var navigator = node.CreateNavigator();
                var iterator = (XPathNodeIterator) navigator.Evaluate(expr);
                if (iterator != null)
                {
                    while (iterator.MoveNext())
                    {
                        if (iterator.Current.IsNode && iterator.Current.NodeType == XPathNodeType.Element)
                        {
                            rootElement = (XElement) iterator.Current.UnderlyingObject;
                            break;
                        }
                    }
                }
                return rootElement;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public static XElement GetOrCreateElementAtPath(this XContainer node, string path)
        {
            try
            {
                if (path == null)
                    throw new ArgumentNullException(nameof(path));
                var parts = path.Split('/');
                XContainer parentNode = node;
                for (var i = 1; i < parts.Length + 1; i++)
                {
                    var p = string.Join("/", parts.Take(i));
                    var element = GetElementByPath(node, p);
                    if (element == null)
                    {
                        var elementName = parts.Skip(Math.Max(0, i - 1)).Take(1).First();
                        element = new XElement(elementName);
                        parentNode.Add(element);
                    }
                    parentNode = element;
                }
                var elem = (XElement) parentNode;
                return elem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
