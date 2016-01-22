using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlDocument : IXmlNode
    {
        void Export(XDocument document);
    }
}