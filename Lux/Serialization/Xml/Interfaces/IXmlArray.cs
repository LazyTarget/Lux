using System.Collections;
using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlArray : IXmlNode, IArray
    {
        new IEnumerable<IXmlNode> Items();

        void AddItem(IXmlNode item);
        //void ClearItems();
    }


    public interface IXmlArray<TNode> : IXmlArray
        where TNode : IXmlNode
    {
        new IEnumerable<TNode> Items();

        void AddItem(TNode item);

        // todo: add more?
    }
}
