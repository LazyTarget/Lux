using System.Collections;
using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlArray : IXmlNode, IEnumerable
    {
        IEnumerable<IXmlNode> Nodes();

        void AddItem(IXmlNode node);
        void Clear();
    }


    public interface IXmlArray<TNode> : IXmlArray, IEnumerable<TNode>
        where TNode : IXmlNode
    {
        void AddItem(TNode node);

        // todo: add more?
    }
}
