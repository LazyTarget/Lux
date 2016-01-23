using System;
using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    [Obsolete]
    public interface IXmlNodeContainer
    {
        IEnumerable<IXmlNode> Nodes();
        void AppendNode(IXmlNode node);
        void ClearNodes();
    }
}
