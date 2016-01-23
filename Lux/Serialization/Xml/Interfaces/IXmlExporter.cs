using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlExporter
    {
        void Export(IXmlObject obj, XElement target);
    }
}