using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlExportable
    {
        void Export(XElement element);
    }
}