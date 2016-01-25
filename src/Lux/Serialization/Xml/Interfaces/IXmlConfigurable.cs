using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlConfigurable
    {
        void Configure(XElement element);
    }
}