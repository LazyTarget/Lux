using System;
using System.Linq;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public class XmlInstantiator : IXmlInstantiator
    {
        private IConverter _converter;
        private ITypeInstantiator _typeInstantiator;

        public XmlInstantiator()
        {
            _converter = new Converter();
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



        public virtual IXmlNode InstantiateNode(XElement element)
        {
            var obj = InstantiateElement(element);
            var node = (IXmlNode) obj;
            return node;
        }

        public virtual object InstantiateElement(XElement element)
        {
            try
            {
                var propertyType = element.GetAttributeValue("type");
                var propertyValue = element.GetAttributeValue("value");
                if (propertyValue == null && !string.IsNullOrEmpty(element.Value))
                    propertyValue = element.Value;

                object value;
                if (!string.IsNullOrEmpty(propertyType))
                {
                    var type = Type.GetType(propertyType);
                    if (type == null)
                    {
                        throw new Exception($"Type '{propertyType}' not found");
                    }
                    else if (typeof(IXmlConfigurable).IsAssignableFrom(type))
                    {
                        var obj = (IXmlConfigurable) InstantiateType(element, type);
                        obj.Configure(element);
                        value = obj;
                    }
                    else if (!type.IsPrimitive)
                    {
                        var temp = Activator.CreateInstance(type);
                        value = temp;
                    }
                    else
                    {
                        value = Converter.Convert(propertyValue, type);
                    }
                }
                else if (element.Attribute("type") != null)
                {
                    // The type to instanciate has not been specified, then use null
                    value = null;
                }
                else
                    value = propertyValue;
                return value;
            }
            catch (Exception ex)
            {
                //_log.Error($"Error when instantiating element", ex);
                throw;
            }
        }
        
        protected virtual object InstantiateType(XElement element, Type type)
        {
            object arguments = null;
            // todo: use xml attributes use for CreateInstance with args

            var obj = _typeInstantiator.Instantiate(type, arguments);
            return obj;
        }



        public virtual void Configure(IXmlConfigurable configurable, XElement element)
        {
            configurable.Configure(element);
        }

        public virtual void ConfigureProperties(IXmlConfigurable configurable, XElement element)
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
                        object value = InstantiateElement(elem);
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

        public virtual void ConfigureArray(IXmlConfigurable configurable, XElement element)
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
                        object value = InstantiateElement(elem);
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

    }
}
