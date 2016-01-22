using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlDocument : IXmlNode
    {
        void Export(XDocument document);
    }
}