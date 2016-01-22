using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public class DynamicObjectXmlConvention : ObjectXmlConventionBase
    {
        public DynamicObjectXmlConvention()
        {
            
        }




        //public override void Configure(IXmlConfigurable configurable, XElement element)
        //{
        //    var obj = configurable as IXmlObject;
        //    if (obj != null)
        //        ConfigureObject(obj, element);
        //}

        protected override void ConfigureObject(IXmlObject obj, XElement element)
        {
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
                        var propertyInfo = obj.GetType().GetProperty(propertyName);
                        if (propertyInfo != null)
                        {
                            value = Converter.Convert(value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(obj, value, null);
                        }
                        else
                            throw new Exception($"Property not found {propertyName}");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }



        //public override void Export(IXmlExportable exportable, XElement element)
        //{
        //    var obj = exportable as IXmlObject;
        //    if (obj != null)
        //        ExportObject(obj, element);
        //}

        protected override void ExportObject(IXmlObject obj, XElement element)
        {
            throw new NotImplementedException();

            var properties = obj.GetProperties();
            foreach (var property in properties)
            {
                if (property == null)
                    continue;
                GetOrUpdateProperty(element, property);
            }
        }


        #region Static

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

        #endregion

    }
}
