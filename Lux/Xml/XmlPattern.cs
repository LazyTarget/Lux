using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public class XmlPattern
    {
        public static readonly XmlPattern Default;

        static XmlPattern()
        {
            Default = new XmlPattern();
        }


        private IConverter _converter;
        private ITypeInstantiator _typeInstantiator;
        private IXmlInstantiator _xmlInstantiator;

        public XmlPattern()
        {
            _converter = new Converter();
            _typeInstantiator = new TypeInstantiator();
            _xmlInstantiator = new XmlInstantiator();
        }

        public virtual IConverter Converter
        {
            get { return _converter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _converter = value;
            }
        }

        public virtual ITypeInstantiator TypeInstantiator
        {
            get { return _typeInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _typeInstantiator = value;
            }
        }

        public virtual IXmlInstantiator Instantiator
        {
            get { return _xmlInstantiator; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _xmlInstantiator = value;
            }
        }



        public virtual void Configure(IXmlConfigurable configurable, XElement element)
        {
            ConfigureObject(configurable, element);
            ConfigureArray(configurable, element);
        }


        protected virtual void ConfigureObject(IXmlConfigurable configurable, XElement element)
        {
            var obj = configurable as IXmlObject;
            if (obj == null)
                return;

            var propertyElems = element.Elements("property").Where(x => x != null).ToList();
            if (propertyElems.Any())
            {
                foreach (var elem in propertyElems)
                {
                    var propertyName = elem.GetAttributeValue("name");
                    if (string.IsNullOrEmpty(propertyName))
                        continue;
                    try
                    {
                        object value = _xmlInstantiator.InstantiateElement(elem);
                        //if (configurable is IHasProperties)
                        //{
                        //    var hasProps = (IHasProperties) configurable;
                        //    hasProps.Properties[propertyName] = value;
                        //}
                        //else
                        {
                            var propertyInfo = configurable.GetType().GetProperty(propertyName);
                            if (propertyInfo != null)
                            {
                                value = _converter.Convert(value, propertyInfo.PropertyType);
                                propertyInfo.SetValue(configurable, value, null);
                            }
                            else
                                throw new Exception($"Property not found {propertyName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        //_log.Error($"Error instantiating property '{propertyName}'", ex);
                    }
                }
            }
        }


        protected virtual void ConfigureArray(IXmlConfigurable configurable, XElement element)
        {
            var array = configurable as IXmlArray;
            if (array == null)
                return;

            // Clear
            array.Clear();

            var itemElems = element.Elements("item").Where(x => x != null).ToList();
            if (itemElems.Any())
            {
                foreach (var elem in itemElems)
                {
                    try
                    {
                        // Append items
                        var node = _xmlInstantiator.InstantiateNode(elem);
                        array.AddItem(node);

                        //object obj = _xmlInstantiator.InstantiateElement(elem);
                        //var value = obj.SafeConvert<T>();
                        //Data.Add(value);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }




        public virtual void Export(IXmlExportable exportable, XElement element)
        {
            ExportObject(exportable, element);
            ExportArray(exportable, element);
        }


        protected virtual void ExportObject(IXmlExportable exportable, XElement element)
        {
            var obj = exportable as IXmlObject;
            if (obj == null)
                return;
            
            var properties = obj.GetPropertyNames();
            foreach (var propertyName in properties)
            {
                var propertyValue = obj.GetProperty(propertyName);
                AddOrUpdateProperty(element, propertyName, propertyValue);
            }
        }


        protected virtual void ExportArray(IXmlExportable exportable, XElement element)
        {
            var array = exportable as IXmlArray;
            if (array == null)
                return;

            element.Elements("item").Remove();

            var nodes = array.Nodes().Where(x => x != null).ToList();
            if (nodes.Any())
            {
                foreach (var node in nodes)
                {
                    var type = node.GetType();
                    var elem = new XElement("item");
                    element.Add(elem);
                    elem.SetAttributeValue("type", type.FullName + ", " + type.Assembly.GetName().Name);
                    node.Export(elem);
                }
            }
        }




        protected static XElement AddOrUpdateProperty(XElement rootElement, string propertyName)
        {
            var propElem = AddOrCreateElement(rootElement, "property", x => x.GetAttributeValue("name") == propertyName);
            propElem.SetAttributeValue("name", propertyName);
            return propElem;
        }

        protected static XElement AddOrUpdateProperty(XElement rootElement, string propertyName, object propertyValue)
        {
            var propElem = AddOrUpdateProperty(rootElement, propertyName);
            if (propertyValue == null)
            {

            }
            else if (propertyValue is IXmlNode)
            {
                var type = propertyValue.GetType();

                var xmlNode = (IXmlNode)propertyValue;
                propElem.SetAttributeValue("type", type.FullName + ", " + type.Assembly.GetName().Name);
                xmlNode.Export(propElem);
            }
            else
            {
                var value = propertyValue != null ? propertyValue.ToString() : null;
                propElem.SetAttributeValue("value", value);
            }
            return propElem;
        }


        protected static XElement AddOrCreateElement(XContainer container, string elementName)
        {
            var elem = AddOrCreateElement(container, elementName, null);
            return elem;
        }

        protected static XElement AddOrCreateElement(XContainer container, string elementName, Func<XElement, bool> predicate)
        {
            List<XElement> elems;
            if (predicate != null)
                elems = container.Elements(elementName).Where(predicate).ToList();
            else
                elems = container.Elements(elementName).ToList();
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
