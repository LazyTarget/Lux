using System;
using System.Linq;
using System.Xml.Linq;
using Lux.Model;
using Lux.Xml;

namespace Lux.Serialization.Xml
{
    public class MirrorObjectXmlConvention : XmlConventionBase
    {
        public MirrorObjectXmlConvention()
        {
            
        }



        
        public override void Configure(IXmlObject obj, XElement source)
        {
            var propertyElems = source.Elements("property").Where(x => x != null).ToList();
            if (propertyElems.Any())
            {
                foreach (var elem in propertyElems)
                {
                    var propertyName = elem.GetAttributeValue("name");
                    if (string.IsNullOrEmpty(propertyName))
                        continue;
                    
                    try
                    {
                        var property = MirrorProperty.Create(obj, propertyName);
                        object value = XmlInstantiator.InstantiateFromElement(elem, obj);
                        value = XmlSettings.Converter.Convert(value, property.Type);
                        property.SetValue(value);

                        //var propertyInfo = target.GetType().GetProperty(propertyName);
                        //if (propertyInfo != null)
                        //{
                        //    value = Converter.Convert(value, propertyInfo.PropertyType);
                        //    propertyInfo.SetValue(target, value, null);
                        //}
                        //else
                        //    throw new Exception($"Property not found {propertyName}");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }


        
        public override void Export(IXmlObject obj, XElement target)
        {
            var mirror = new MirrorObjectModel(obj);

            //var properties = target.GetProperties();
            var properties = mirror.GetProperties();
            foreach (var property in properties)
            {
                if (property == null)
                    continue;
                GetOrUpdateProperty(target, property);
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
