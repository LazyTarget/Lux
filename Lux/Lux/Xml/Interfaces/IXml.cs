using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXmlConfigurable
    {
        void Configure(XElement element);
    }

    public interface IXmlExportable
    {
        void Export(XElement element);
    }


    public interface IXmlNode : INode, IXmlConfigurable, IXmlExportable
    {
        //void Configure(XElement element);

        //void Export(XElement element);
    }

    public interface IXmlArray : IXmlNode
    {
        
    }

    public interface IXmlObject : IXmlNode
    {
        
    }

    public interface IXmlDocument : IXmlNode
    {
        void Export(XDocument document);
    }
}
