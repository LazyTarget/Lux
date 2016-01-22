using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class DefaultObjectXmlConvention : XmlConventionBase
    {
        public override void Configure(IXmlConfigurable configurable, XElement element)
        {
            ConfigureObject(configurable, element);
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
                        object value = XmlInstantiator.InstantiateElement(elem);
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
                                value = Converter.Convert(value, propertyInfo.PropertyType);
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




        public override void Export(IXmlExportable exportable, XElement element)
        {
            ExportObject(exportable, element);
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



        #region Static


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

        #endregion

    }
}
