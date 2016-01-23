using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlDocument : IXmlNode
    {
        void Configure(XDocument document);
        void Export(XDocument document);
    }
}