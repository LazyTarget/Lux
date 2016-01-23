using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlConfigurator
    {
        void Configure(IXmlObject obj, XElement source);
    }
}