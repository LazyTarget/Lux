using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Data;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Model.Xml
{
    public class XmlObjectModel : IObjectModel
    {
        public XmlObjectModel(XElement element)
        {
            Element = element;
            PropertyParser = new XElementPropertyParser();
        }

        public XElement Element { get; private set; }

        public IDataStore<XElement, IEnumerable<IProperty>> PropertyParser { get; private set; } 


        public IEnumerable<IProperty> GetProperties()
        {
            var properties = PropertyParser.Load(Element);
            return properties;
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
                // todo: define?
                throw new KeyNotFoundException($"Property '{name}' not found");
            }
        }

        public void ClearProperties()
        {
            IEnumerable<IProperty> value = null;
            PropertyParser.Save(Element, value);
        }
    }


    public class XElementPropertyParser : IDataStore<XElement, IEnumerable<IProperty>>
    {
        object IDataStore<XElement>.Load(XElement key)
        {
            return Load(key);
        }

        public IEnumerable<IProperty> Load(XElement element)
        {
            var properties = new List<IProperty>();
            var propertyElems = element.Elements("property").Where(x => x != null).ToList();
            if (propertyElems.Any())
            {
                foreach (var elem in propertyElems)
                {
                    var propertyName = elem.GetAttributeValue("name");
                    if (string.IsNullOrEmpty(propertyName))
                        continue;
                    var propertyValueType = elem.GetAttributeValue("type");
                    var type = !string.IsNullOrEmpty(propertyValueType)
                        ? Type.GetType(propertyValueType)
                        : null;
                    IProperty property = type != null
                        ? new StronglyTypedProperty(propertyName, type)
                        : new Property(propertyName);
                    properties.Add(property);
                }
            }
            return properties;
        }


        object IDataStore<XElement>.Save(XElement key, object value)
        {
            var val = (IEnumerable<IProperty>) value;
            return Save(key, val);
        }

        public IEnumerable<IProperty> Save(XElement element, IEnumerable<IProperty> value)
        {
            element.Elements().Where(x=>x.Name == "property").Remove();

            var properties = value.ToList();
            foreach (var property in properties)
            {
                if (property == null)
                    continue;
                GetOrUpdateProperty(element, property);
            }
            return properties;
        }


        protected static XElement GetOrUpdateProperty(XElement rootElement, IProperty property)
        {
            var propElem = rootElement.GetOrCreateElement("property", x => x.GetAttributeValue("name") == property.Name);
            propElem.SetAttributeValue("name", property.Name);      // todo: escape attribute values?

            var propertyValue = property.Value;
            if (propertyValue == null)
            {

            }
            else if (propertyValue is IXmlExportable)
            {
                var type = propertyValue.GetType();
                var typeString = type.FullName + ", " + type.Assembly.GetName().Name;

                var exportable = (IXmlExportable)propertyValue;
                propElem.SetAttributeValue("type", typeString);
                exportable.Export(propElem);
            }
            else
            {
                var value = propertyValue != null ? propertyValue.ToString() : null;
                propElem.SetAttributeValue("value", value);
            }
            return propElem;
        }


    }
}
