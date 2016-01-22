using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlExporter
    {
        void Export(IXmlExportable exportable, XElement element);
    }
}