using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXNodeInterpreter
    {
        XNode GetNode();

        IXNodeInterpreter<TNodeType, IXNodeInterpreter<XNode>> To<TNodeType>()
            where TNodeType : XNode;

        IXNodeInterpreterIterator<XNode, IXNodeInterpreter> Children();

        IXNodeInterpreter<XNode, IXNodeInterpreter<XNode>> GetChild(int index);
    }


    public interface IXNodeInterpreter<out TNode> : IXNodeInterpreter
    {
        new TNode GetNode();

        new IXNodeInterpreter<TNodeType, IXNodeInterpreter<TNode>> To<TNodeType>()
            where TNodeType : XNode;

        new IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode>> Children();

        new IXNodeInterpreter<XNode, IXNodeInterpreter<TNode>> GetChild(int index);
    }


    public interface IXNodeInterpreter<out TNode, out TParent> : IXNodeInterpreter<TNode>, IFluentReturn<TParent>
    {
        new IXNodeInterpreter<TNodeType, IXNodeInterpreter<TNode, TParent>> To<TNodeType>()
            where TNodeType : XNode;

        new IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>> Children();

        new IXNodeInterpreter<XNode, IXNodeInterpreter<TNode, TParent>> GetChild(int index);
    }
}
