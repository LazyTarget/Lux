using System.Xml.Linq;

namespace Lux.Xml
{
    public interface IXmlInstantiator
    {
        IXmlNode InstantiateNode(XElement element);

        object InstantiateElement(XElement element);
    }
}
