using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Lux.Extensions;
using Lux.Interfaces;
using Lux.Xml;

namespace Lux.Serialization.Xml
{
    public class XmlSerializer : ISerializer
    {
        public XmlSerializer()
        {
            Culture = Framework.CultureInfo;
            TypeInstantiator = Framework.TypeInstantiator;
            //Converter = Framework.Converter;
            Converter = new Converter(TypeInstantiator);
        }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public string DateFormat { get; set; }

        public CultureInfo Culture { get; set; }

        public ITypeInstantiator TypeInstantiator { get; set; }

        public IConverter Converter { get; set; }

        
        /// <summary>
        /// Serialize the object as XML
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>XML as string</returns>
        public string Serialize(object obj)
        {
            var doc = new XDocument();
            var t = obj.GetType();
            var name = t.IsAnonymousType()
                ? "Object"
                : t.Name;

            var options = t.GetAttribute<SerializeAsAttribute>();
            if (options != null)
            {
                name = options.TransformName(options.Name ?? name);
            }

            var root = new XElement(name.AsNamespaced(Namespace));
            if (obj is IList)
            {
                string itemTypeName = "";

                foreach (object item in (IList) obj)
                {
                    Type type = item.GetType();
                    SerializeAsAttribute opts = type.GetAttribute<SerializeAsAttribute>();

                    if (opts != null)
                    {
                        itemTypeName = opts.TransformName(opts.Name ?? name);
                    }

                    if (itemTypeName == "")
                    {
                        itemTypeName = type.Name;
                    }

                    XElement instance = new XElement(itemTypeName.AsNamespaced(Namespace));

                    this.Map(instance, item);
                    root.Add(instance);
                }
            }
            else
            {
                this.Map(root, obj);
            }

            if (!RootElement.IsNullOrEmpty())
            {
                XElement wrapper = new XElement(this.RootElement.AsNamespaced(Namespace), root);
                doc.Add(wrapper);
            }
            else
            {
                doc.Add(root);
            }

            return doc.ToString();
        }

        private void Map(XContainer root, object obj)
        {
            Type objType = obj.GetType();
            IEnumerable<PropertyInfo> props = from p in objType.GetProperties()
                                              let indexAttribute = p.GetAttribute<SerializeAsAttribute>()
                                              where p.CanRead && p.CanWrite
                                              orderby indexAttribute == null
                                                  ? int.MaxValue
                                                  : indexAttribute.Index
                                              select p;
            SerializeAsAttribute globalOptions = objType.GetAttribute<SerializeAsAttribute>();

            foreach (PropertyInfo prop in props)
            {
                string name = prop.Name;
                object rawValue = prop.GetValue(obj, null);

                if (rawValue == null)
                {
                    continue;
                }

                string value = this.GetSerializedValue(rawValue);
                Type propType = prop.PropertyType;
                bool useAttribute = false;
                SerializeAsAttribute settings = prop.GetAttribute<SerializeAsAttribute>();

                if (settings != null)
                {
                    name = !settings.Name.IsNullOrEmpty()
                        ? settings.Name
                        : name;
                    useAttribute = settings.Attribute;
                }

                SerializeAsAttribute options = prop.GetAttribute<SerializeAsAttribute>();

                if (options != null)
                {
                    name = options.TransformName(name);
                }
                else if (globalOptions != null)
                {
                    name = globalOptions.TransformName(name);
                }

                XName nsName = name.AsNamespaced(Namespace);
                var element = new XElement(nsName);

                if (propType.IsPrimitive || propType.IsValueType || propType == typeof(string))
                {
                    if (useAttribute)
                    {
                        root.Add(new XAttribute(name, value));
                        continue;
                    }

                    element.Value = value;
                }
                else if (rawValue is IList)
                {
                    var list = (IList) rawValue;
                    var itemTypeName = "";
                    foreach (var item in list)
                    {
                        if (itemTypeName == "")
                        {
                            var type = item.GetType();
                            var setting = type.GetAttribute<SerializeAsAttribute>();

                            itemTypeName = setting != null && !setting.Name.IsNullOrEmpty()
                                ? setting.Name
                                : type.Name;
                        }

                        var instance = new XElement(itemTypeName.AsNamespaced(Namespace));
                        Map(instance, item);
                        element.Add(instance);
                    }
                }
                else
                {
                    Map(element, rawValue);
                }

                root.Add(element);
            }
        }


        private string GetSerializedValue(object obj)
        {
            object output = obj;

            if (obj is DateTime && !DateFormat.IsNullOrEmpty())
            {
                output = ((DateTime) obj).ToString(DateFormat, Culture);
            }

            if (obj is bool)
            {
                output = ((bool) obj).ToString(Culture).ToLower();
            }

            if (IsNumeric(obj))
            {
                return SerializeNumber(obj, Culture);
            }

            return output.ToString();
        }

        private static string SerializeNumber(object number, CultureInfo culture)
        {
            string res;
            if (number is long)
                res = ((long) number).ToString(culture);
            else if (number is ulong)
                res = ((ulong) number).ToString(culture);
            else if (number is int)
                res = ((int) number).ToString(culture);
            else if (number is uint)
                res = ((uint) number).ToString(culture);
            else if (number is decimal)
                res = ((decimal) number).ToString(culture);
            else if (number is float)
                res = ((float) number).ToString(culture);
            else
            {
                var d = Convert.ToDouble(number, culture);
                res = d.ToString("r", culture);
            }
            return res;
        }


        /// <summary>
        /// Determines if a given object is numeric in any way
        /// (can be integer, double, null, etc).
        /// </summary>
        private static bool IsNumeric(object value)
        {
            if (value is sbyte)
                return true;
            if (value is byte)
                return true;
            if (value is short)
                return true;
            if (value is ushort)
                return true;
            if (value is int)
                return true;
            if (value is uint)
                return true;
            if (value is long)
                return true;
            if (value is ulong)
                return true;
            if (value is float)
                return true;
            if (value is double)
                return true;
            if (value is decimal)
                return true;
            return false;
        }

        

        protected virtual object Map(object x, XElement root)
        {
            var objType = x.GetType();
            var props = objType.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                var type = prop.PropertyType;
                var typeIsPublic = type.IsPublic || type.IsNestedPublic;
                if (!typeIsPublic || !prop.CanWrite)
                {
                    continue;
                }

                XName name = prop.Name;
                var options = prop.GetAttribute<DeserializeAsAttribute>();
                if (options != null)
                {
                    name = options.Name.AsNamespaced(Namespace);
                }
                else
                {
                    name = prop.Name.AsNamespaced(Namespace);
                }

                var value = GetValueFromXml(root, name, prop);
                if (value == null)
                {
                    // special case for inline list items
                    if (type.IsGenericType)
                    {
                        var genericType = type.GetGenericArguments()[0];
                        var first = GetElementByName(root, genericType.Name);
                        //var list = (IList) Activator.CreateInstance(type);
                        var list = (IList) TypeInstantiator.Instantiate(type);

                        if (first != null && root != null)
                        {
                            var elements = root.Elements(first.Name);
                            PopulateListFromElements(genericType, elements, list);
                        }
                        prop.SetValue(x, list, null);
                    }
                    continue;
                }

                // check for nullable and extract underlying type
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // if the value is empty, set the property to null...
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        prop.SetValue(x, null, null);
                        continue;
                    }

                    type = type.GetGenericArguments()[0];
                }

                if (type == typeof(bool))
                {
                    var toConvert = value.ToString().ToLower();
                    var b = XmlConvert.ToBoolean(toConvert);
                    prop.SetValue(x, b, null);
                }
                else if (type.IsPrimitive)
                {
                    value = Converter.Convert(value, type);     // todo: param for Culture
                    //prop.SetValue(x, value.ChangeType(type, Culture), null);
                    prop.SetValue(x, value);
                }
                else if (type.IsEnum)
                {
                    var converted = type.FindEnumValue(value.ToString(), Culture);
                    prop.SetValue(x, converted, null);
                }
                else if (type == typeof(Uri))
                {
                    var uri = new Uri(value.ToString(), UriKind.RelativeOrAbsolute);
                    prop.SetValue(x, uri, null);
                }
                else if (type == typeof(string))
                {
                    prop.SetValue(x, value, null);
                }
                else if (type == typeof(DateTime))
                {
                    value = !DateFormat.IsNullOrEmpty()
                        ? DateTime.ParseExact(value.ToString(), DateFormat, Culture)
                        : DateTime.Parse(value.ToString(), Culture);

                    prop.SetValue(x, value, null);
                }
                else if (type == typeof(DateTimeOffset))
                {
                    var toConvert = value.ToString();
                    if (!toConvert.IsNullOrEmpty())
                    {
                        DateTimeOffset deserialisedValue;

                        try
                        {
                            deserialisedValue = XmlConvert.ToDateTimeOffset(toConvert);
                            prop.SetValue(x, deserialisedValue, null);
                        }
                        catch (Exception)
                        {
                            object result;

                            if (TryGetFromString(toConvert, out result, type))
                            {
                                prop.SetValue(x, result, null);
                            }
                            else
                            {
                                //fallback to parse
                                deserialisedValue = DateTimeOffset.Parse(toConvert);
                                prop.SetValue(x, deserialisedValue, null);
                            }
                        }
                    }
                }
                else if (type == typeof(decimal))
                {
                    value = decimal.Parse(value.ToString(), this.Culture);
                    prop.SetValue(x, value, null);
                }
                else if (type == typeof(Guid))
                {
                    string raw = value.ToString();

                    value = string.IsNullOrEmpty(raw)
                        ? Guid.Empty
                        : new Guid(value.ToString());

                    prop.SetValue(x, value, null);
                }
                else if (type == typeof(TimeSpan))
                {
                    var timeSpan = XmlConvert.ToTimeSpan(value.ToString());
                    prop.SetValue(x, timeSpan, null);
                }
                else if (type.IsGenericType)
                {
                    var t = type.GetGenericArguments()[0];
                    //var list = (IList) Activator.CreateInstance(type);
                    var list = (IList) TypeInstantiator.Instantiate(type);
                    var container = GetElementByName(root, prop.Name.AsNamespaced(Namespace));
                    if (container.HasElements)
                    {
                        var first = container.Elements().FirstOrDefault();
                        if (first != null)
                        {
                            var elements = container.Elements(first.Name);
                            PopulateListFromElements(t, elements, list);
                        }
                    }

                    prop.SetValue(x, list, null);
                }
                else if (type.IsSubclassOfRawGeneric(typeof(List<>)))
                {
                    // handles classes that derive from List<T>
                    // e.g. a collection that also has attributes
                    object list = HandleListDerivative(root, prop.Name, type);

                    prop.SetValue(x, list, null);
                }
                else
                {
                    //fallback to type converters if possible
                    object result;

                    if (TryGetFromString(value.ToString(), out result, type))
                    {
                        prop.SetValue(x, result, null);
                    }
                    else
                    {
                        // nested property classes
                        if (root != null)
                        {
                            var element = GetElementByName(root, name);
                            if (element != null)
                            {
                                object item = CreateAndMap(type, element);

                                prop.SetValue(x, item, null);
                            }
                        }
                    }
                }
            }

            return x;
        }

        private static bool TryGetFromString(string inputString, out object result, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(typeof(string)))
            {
                result = (converter.ConvertFromInvariantString(inputString));
                return true;
            }
            result = null;
            return false;
        }

        private void PopulateListFromElements(Type t, IEnumerable<XElement> elements, IList list)
        {
            foreach (var elem in elements)
            {
                var item = CreateAndMap(t, elem);
                list.Add(item);
            }
        }

        private object HandleListDerivative(XElement root, string propName, Type type)
        {
            var t = type.IsGenericType
                ? type.GetGenericArguments()[0]
                : type.BaseType.GetGenericArguments()[0];
            //var list = (IList) Activator.CreateInstance(type);
            var list = (IList) TypeInstantiator.Instantiate(type);
            var elements = root.Descendants(t.Name.AsNamespaced(Namespace)).ToList();
            string name = t.Name;

            if (!elements.Any())
            {
                XName lowerName = name.ToLower().AsNamespaced(Namespace);
                elements = root.Descendants(lowerName).ToList();
            }

            if (!elements.Any())
            {
                XName camelName = name.ToCamelCase(Culture).AsNamespaced(Namespace);
                elements = root.Descendants(camelName).ToList();
            }

            if (!elements.Any())
            {
                elements = root.Descendants()
                               .Where(e => e.Name.LocalName.RemoveUnderscoresAndDashes() == name)
                               .ToList();
            }

            if (!elements.Any())
            {
                XName lowerName = name.ToLower().AsNamespaced(Namespace);

                elements = root.Descendants()
                               .Where(e => e.Name.LocalName.RemoveUnderscoresAndDashes() == lowerName)
                               .ToList();
            }

            PopulateListFromElements(t, elements, list);

            // get properties too, not just list items
            // only if this isn't a generic type
            if (!type.IsGenericType)
            {
                Map(list, root.Element(propName.AsNamespaced(Namespace)) ?? root);
                // when using RootElement, the heirarchy is different
            }

            return list;
        }

        protected virtual object CreateAndMap(Type t, XElement element)
        {
            object item;

            if (t == typeof(string))
            {
                item = element.Value;
            }
            else if (t.IsPrimitive)
            {
                var value = Converter.Convert(element.Value, t);        // todo: param for Culture
                item = value;
            }
            else
            {
                item = Activator.CreateInstance(t);
                this.Map(item, element);
            }

            return item;
        }

        protected virtual object GetValueFromXml(XElement root, XName name, PropertyInfo prop)
        {
            //Check for the DeserializeAs attribute on the property
            var isAttribute = false;
            var options = prop.GetAttribute<DeserializeAsAttribute>();
            if (options != null)
            {
                name = options.Name ?? name;
                isAttribute = options.Attribute;
            }
            if (isAttribute)
            {
                var attributeVal = GetAttributeByName(root, name);
                if (attributeVal != null)
                {
                    return attributeVal.Value;
                }
            }

            
            object val = null;
            if (root != null)
            {
                var element = GetElementByName(root, name);
                if (element == null)
                {
                    var attribute = GetAttributeByName(root, name);
                    if (attribute != null)
                    {
                        val = attribute.Value;
                    }
                }
                else
                {
                    if (!element.IsEmpty || element.HasElements || element.HasAttributes)
                    {
                        val = element.Value;
                    }
                }
            }
            return val;
        }

        protected virtual XElement GetElementByName(XElement root, XName name)
        {
            XName lowerName = name.LocalName.ToLower().AsNamespaced(name.NamespaceName);
            XName camelName = name.LocalName.ToCamelCase(Culture).AsNamespaced(name.NamespaceName);

            if (root.Element(name) != null)
            {
                return root.Element(name);
            }

            if (root.Element(lowerName) != null)
            {
                return root.Element(lowerName);
            }

            if (root.Element(camelName) != null)
            {
                return root.Element(camelName);
            }

            if (name == "Value".AsNamespaced(name.NamespaceName))
            {
                return root;
            }

            // try looking for element that matches sanitized property name (Order by depth)
            var result = root.Descendants()
                .OrderBy(d => d.Ancestors().Count())
                .FirstOrDefault(d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName);
            if (result == null)
            {
                result = root.Descendants()
                    .OrderBy(d => d.Ancestors().Count())
                    .FirstOrDefault(d => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName.ToLower());
            }
            return result;
        }

        protected virtual XAttribute GetAttributeByName(XElement root, XName name)
        {
            var names = new List<XName>
            {
                name.LocalName,
                name.LocalName.ToLower().AsNamespaced(name.NamespaceName),
                name.LocalName.ToCamelCase(Culture).AsNamespaced(name.NamespaceName),
            };

            var attr = root.DescendantsAndSelf()
                .OrderBy(d => d.Ancestors().Count())
                .Attributes()
                .FirstOrDefault(d => names.Contains(d.Name.LocalName.RemoveUnderscoresAndDashes()));
            return attr;
        }
    }
}
