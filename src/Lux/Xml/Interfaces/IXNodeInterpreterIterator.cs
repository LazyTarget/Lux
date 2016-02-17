using System.Collections.Generic;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXNodeInterpreterIterator
    {
        IEnumerable<IXNodeInterpreter> Enumerate();
    }


    public interface IXNodeInterpreterIterator<out TNode> : IXNodeInterpreterIterator
        //where TNode : XNode
    {
        new IEnumerable<IXNodeInterpreter<TNode>> Enumerate();
    }


    public interface IXNodeInterpreterIterator<out TNode, out TParent> : IXNodeInterpreterIterator<TNode>, IFluentReturn<TParent>
        //where TNode : XNode
    {
        IXNodeInterpreter<TNode, IXNodeInterpreterIterator<TNode, TParent>> GetAtIndex(int index);

        new IEnumerable<IXNodeInterpreter<TNode, IXNodeInterpreterIterator<TNode, TParent>>> Enumerate();
    }
}