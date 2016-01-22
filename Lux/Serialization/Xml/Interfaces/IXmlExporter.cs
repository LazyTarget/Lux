using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlExporter
    {
        void Export(IXmlExportable exportable, XElement element);
    }
}