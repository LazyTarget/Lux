using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lux.Model.Xml
{
    public interface IXmlObjectModelPropertyParser
    {
        IEnumerable<IProperty> Parse(XElement element);
        IProperty DefineProperty(XElement element, string propertyName, Type type, object value, bool isReadOnly);
        void ClearProperties(XElement element);
    }
}