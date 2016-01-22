using System.Collections.Generic;

namespace Lux.Xml
{
    public interface IXmlObject : IXmlNode, IEnumerable<KeyValuePair<string, object>>
    {
        IEnumerable<string> GetPropertyNames();
        bool HasProperty(string name);
        object GetProperty(string name);
        void SetProperty(string name, object value);
        void Clear();
    }
}