using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlExportable
    {
        void Export(XElement element);
    }
}