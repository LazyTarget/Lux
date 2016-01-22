using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public interface IXmlInstantiator
    {
        IXmlNode InstantiateNode(XElement element);

        object InstantiateElement(XElement element);
    }
}
