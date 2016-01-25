using System;
using System.Linq;
using System.Xml.Linq;
using Lux.Xml;

namespace Lux.Serialization.Xml
{
    public class XmlSerializer : IXmlSerializer, IXmlInstantiator
    {
        private XmlSettings _xmlSettings;

        public XmlSerializer()
            : this(new XmlSettings())
        {
            //_xmlSettings.XmlSerializer = this;
        }

        public XmlSerializer(XmlSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            _xmlSettings = settings;
        }


        public virtual XmlSettings XmlSettings
        {
            get { return _xmlSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _xmlSettings = value;
            }
        }


        public XDocument Serialize(IXmlDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var result = new XDocument();
            var obj = document as XmlObject;

            var pattern = obj?.Pattern ?? XmlSettings.XmlPattern;

            if (obj != null)
            {
                var target = GetOrCreateRootElem(result, document.RootElementName);
                pattern.Export(document, target);

                //_xmlSettings.XmlPattern.Export(obj, target);
                //obj.Pattern.
                
                //document.Export(document);
                //obj.Pattern.Export(obj, obj.Element);
                //result = obj.Element;
            }
            else
            {
                throw new NotSupportedException();
            }
            return result;
        }

        protected virtual void Serialize(IXmlNode node, XElement target)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var obj = node as XmlObject;
            if (obj != null)
            {
                obj.Pattern.Export(obj, target);
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        public IXmlNode Deserialize(XNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            IXmlNode result = null;
            var element = node as XElement;
            if (element != null)
            {
                result = InstantiateNode(element);
            }
            else
            {
                throw new NotSupportedException();
            }
            return result;
        }


        public TNode Deserialize<TNode>(XNode node) 
            where TNode : IXmlNode
        {
            var obj = Deserialize(node);
            var res = (TNode) obj;
            return res;
        }




        public virtual IXmlNode InstantiateNode(XElement element)
        {
            var obj = InstantiateFromElement(element, null);
            var node = (IXmlNode)obj;
            return node;
        }

        public virtual IXmlNode InstantiateNode(XElement element, IXmlNode parent)
        {
            var obj = InstantiateFromElement(element, parent);
            var node = (IXmlNode) obj;
            return node;
        }

        public virtual object InstantiateFromElement(XElement element, IXmlNode parent)
        {
            try
            {
                var propertyType = element.GetAttributeValue("type");
                var propertyValue = element.GetAttributeValue("value");
                if (propertyValue == null && !string.IsNullOrEmpty(element.Value))
                    propertyValue = element.Value;

                Type type = null;
                if (!string.IsNullOrEmpty(propertyType))
                {
                    type = Type.GetType(propertyType);
                    if (type == null)
                        throw new Exception($"Type '{propertyType}' not found");
                }
                else if (element.Attribute("type") != null)
                {
                    // The type to instanciate has not been specified, then use null
                    type = null;
                }
                else
                {
                    if (element.Element("item") != null)
                        type = typeof (XmlList<XmlObject>);
                    else
                        type = typeof (XmlObject);
                }

                object value;
                if (!string.IsNullOrEmpty(propertyValue))
                {
                    value = propertyValue;
                }
                else if (type != null)
                {
                    if (typeof(IXmlObject).IsAssignableFrom(type))
                    {
                        var instance = (IXmlObject)InstantiateType(element, type);
                        var obj = instance as XmlObject;
                        if (obj != null)
                        {
                            obj.Element = element;

                            var parentObj = parent as XmlObject;
                            if (parentObj != null)
                            {
                                obj.Pattern = parentObj.Pattern;
                            }

                            if (obj.Pattern != null)
                            {
                                obj.Pattern.Configure(obj, element);
                            }
                            value = obj;
                        }
                        else if (instance is IXmlConfigurable)
                        {
                            var configurable = (IXmlConfigurable) instance;
                            configurable.Configure(element);
                            value = configurable;
                        }
                        else
                        {
                            value = instance;
                        }
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
                        value = XmlSettings.Converter.Convert(propertyValue, type);
                    }
                }
                else
                    value = propertyValue;
                return value;
            }
            catch (Exception ex)
            {
                //_log.Error($"Error when instantiating obj", ex);
                throw;
            }
        }
        
        protected virtual object InstantiateType(XElement element, Type type)
        {
            object[] arguments = null;
            // todo: use xml attributes use for CreateInstance with args

            var obj = XmlSettings.TypeInstantiator.Instantiate(type, arguments);
            return obj;
        }





        
        private XElement GetRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.Element(rootElementName);
                else
                    rootElement = document.Element(rootElementName);
            }
            return rootElement;
        }

        private XElement GetOrCreateRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.GetOrCreateElement(rootElementName);
                else
                    rootElement = document.GetOrCreateElement(rootElementName);
            }
            return rootElement;
        }

    }
}
