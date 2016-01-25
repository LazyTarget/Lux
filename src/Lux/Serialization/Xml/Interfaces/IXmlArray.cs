using System;
using System.Collections;
using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlArray : IXmlNode, IArray
    {
        //IEnumerable<object> Items();

        //void AddItem(object item);
        //void ClearItems();
    }
    
    public interface IXmlArray<TNode> : IXmlArray, IArray<TNode>
        where TNode : IXmlNode
    {
        //IEnumerable<TNode> Items();

        //new void AddItem(TNode item);

        // todo: add more?
    }
}
