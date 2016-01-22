using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlConfigurable
    {
        void Configure(XElement element);
    }
}