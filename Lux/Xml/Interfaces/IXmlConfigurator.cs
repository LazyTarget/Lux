using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlConfigurator
    {
        void Configure(IXmlConfigurable configurable, XElement element);
    }
}