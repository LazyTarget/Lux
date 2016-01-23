using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlInstantiator
    {
        IXmlNode InstantiateNode(IXmlNode parent, XElement element);

        object InstantiateElement(IXmlNode parent, XElement element);
    }
}
