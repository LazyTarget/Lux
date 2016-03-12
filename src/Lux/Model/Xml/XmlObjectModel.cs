using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Model.Xml
{
    public class XmlObjectModel : IObjectModel
    {
        private IDictionary<string, IProperty> _properties; 

        public XmlObjectModel(XElement element)
            : this(element, new XElementPropertyParser())
        {
        }

        public XmlObjectModel(XElement element, IXmlObjectModelPropertyParser propertyParser)
        {
            _properties = new Dictionary<string, IProperty>();
            Element = element;
            PropertyParser = propertyParser;
        }

        public XElement Element { get; private set; }

        public IXmlObjectModelPropertyParser PropertyParser { get; private set; } 


        public IEnumerable<IProperty> GetProperties()
        {
            // Use cached properties
            var result = new Dictionary<string, IProperty>();
            var parsed = PropertyParser.Parse(Element).ToDictionary(x => x.Name, x => x);

            for (var i = 0; i < _properties.Count; i++)
            {
                var pair = _properties.ElementAt(i);
                if (!parsed.ContainsKey(pair.Key))
                {
                    // Property removed from element, remove from cache...
                    _properties.Remove(pair);
                    continue;
                }
                // Append to result set
                result[pair.Key] = pair.Value;
            }

            foreach (var property in parsed.Values)
            {
                if (result.ContainsKey(property.Name))
                {
                    // Property exists in cache, ignore...
                    continue;
                }
                else
                {
                    // New property, add to cache and result set...
                    result[property.Name] =
                        _properties[property.Name] = property;
                }
            }
            return result.Values;
        }

        public IProperty GetProperty(string name)
        {
            var property = GetProperties().FirstOrDefault(x => x.Name == name);
            return property;
        }

        public void SetPropertyValue(string name, object value)
        {
            var property = GetProperty(name);
            if (property != null)
            {
                property.SetValue(value);
            }
            else
            {
                Type type = null;
                property = PropertyParser.DefineProperty(Element, name, type, value, false);
                _properties[property.Name] = property;
            }
        }

        public void ClearProperties()
        {
            PropertyParser.ClearProperties(Element);
            _properties.Clear();
        }

    }
}
