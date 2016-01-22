using System.Collections.Generic;
using Lux.Interfaces;
using Lux.Serialization;

namespace Lux.Xml
{
    public interface IXmlObject : IXmlNode, IEnumerable<IProperty>
    {
        IEnumerable<string> GetPropertyNames();
        bool HasProperty(string name);
        IProperty GetProperty(string name);
        void SetPropertyValue(string name, object value);
        void Clear();
    }
}