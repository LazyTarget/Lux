using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Serialization.Xml;
using Lux.Xml;

namespace Lux.Model.Xml
{
    public class XElementPropertyParser : IXmlObjectModelPropertyParser
    {
        public IEnumerable<IProperty> Parse(XElement element)
        {
            var properties = new List<IProperty>();
            var propertyElems = element.Elements("property").Where(x => x != null).ToList();
            if (propertyElems.Any())
            {
                foreach (var elem in propertyElems)
                {
                    IProperty property = new XmlProperty(elem);
                    properties.Add(property);
                }
            }
            return properties;
        }

        public IProperty DefineProperty(XElement element, string propertyName, Type type, object value, bool isReadOnly)
        {
            var propElem = new XElement("property");
            element.Add(propElem);

            var property = new XmlProperty(propElem);
            property.Name = propertyName;
            property.ReadOnly = isReadOnly;
            property.Type = type;
            property.SetValue(value);
            return property;
        }

        public void ClearProperties(XElement element)
        {
            element.Elements().Where(x => x.Name == "property").Remove();
        }




        public class XmlProperty : IProperty
        {
            private readonly Assignable<string> _name = new Assignable<string>(); 
            private readonly Assignable<object> _value = new Assignable<object>();
            private readonly Assignable<Type> _type = new Assignable<Type>(); 
            private readonly XElement _element;

            public XmlProperty(XElement element)
            {
                _element = element;
                if (element == null)
                    throw new ArgumentNullException(nameof(element));
            }


            public string Name
            {
                get
                {
                    string value;
                    if (_name.Assigned)
                    {
                        value = _name.Value;
                    }
                    else
                    {
                        value = _element?.GetAttributeValue("name");
                    }
                    return value;
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentNullException(nameof(value));
                    var name = value;
                    _element?.SetAttr("name", name);
                    _name.Value = name;
                }
            }

            public Type Type
            {
                get
                {
                    Type value = null;
                    if (_type.Assigned)
                    {
                        // todo: what if changed type attr after?
                        value = _type.Value;
                    }
                    else
                    {
                        var propType = _element?.GetAttributeValue("type");
                        value = !string.IsNullOrEmpty(propType)
                            ? Type.GetType(propType)
                            : null;
                    }
                    return value;
                }
                set
                {
                    var type = value;
                    if (type != null)
                    {
                        var typeString = type.FullName +
                                         (type.Assembly.IsFullyTrusted
                                             ? ""
                                             : ", " + type.Assembly.GetName().Name);
                        _element?.SetAttr("type", typeString);
                    }
                    else
                    {
                        _element?.Attribute("type")?.Remove();
                    }
                    _type.Value = type;
                }
            }

            public bool ReadOnly { get; set; }

            public object Value
            {
                get
                {
                    object value;
                    if (_value.Assigned)
                    {
                        value = _value.Value;
                    }
                    else
                    {
                        IXmlInstantiator instantiator = new CustomXmlSerializer();
                        value = instantiator.InstantiateFromElement(_element, null);

                        AssertIsAssignable(value);
                        _value.Value = value;
                    }
                    return value;
                }
                private set
                {
                    AssertIsAssignable(value);
                    _value.Value = value;

                    if (value is IXmlExportable)
                    {
                        var exportable = (IXmlExportable) value;
                        exportable.Export(_element);
                    }
                    else
                        _element.SetAttributeValue("value", value);
                }
            }

            public void SetValue(object value)
            {
                Value = value;
            }


            protected void AssertIsAssignable(object value)
            {
                if (value != null && Type != null)
                {
                    var type = value.GetType();
                    var valid = this.Type.IsAssignableFrom(type);
                    if (!valid)
                        throw new InvalidOperationException("Invalid property value. Doesn't match the required type");
                }
            }

        }
    }
}