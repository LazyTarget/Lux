using System.Collections.Generic;

namespace Lux.Serialization.Xml
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