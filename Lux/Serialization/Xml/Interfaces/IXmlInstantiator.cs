using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlInstantiator
    {
        IXmlNode InstantiateNode(XElement element);

        IXmlNode InstantiateNode(XElement element, IXmlNode parent);

        object InstantiateFromElement(XElement element, IXmlNode parent);
    }
}
