using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlConfigurator
    {
        void Configure(IXmlConfigurable configurable, XElement element);
    }
}