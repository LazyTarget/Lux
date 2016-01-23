using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlNodeContainer
    {
        IEnumerable<IXmlNode> Nodes();
        void AppendNode(IXmlNode node);
        void ClearNodes();
    }
}
