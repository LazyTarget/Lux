using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlDocument : IXmlObject, IXmlNode, IXmlConfigurable, IXmlExportable
    {
        string RootElementName { get; }

        void Configure(XDocument document);
        void Export(XDocument document);
    }
}