using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXNodeNavigator
    {
        XNode GetNode();

        IXNodeNavigator<TNodeType, IXNodeNavigator<XNode>> To<TNodeType>()
            where TNodeType : XNode;

        IXNodeNavigator<XNode, IXNodeNavigator<XNode>> GetChild(int index);
    }

    public interface IXNodeNavigator<out TNode> : IXNodeNavigator
    {
        new TNode GetNode();

        new IXNodeNavigator<TNodeType, IXNodeNavigator<TNode>> To<TNodeType>()
            where TNodeType : XNode;

        new IXNodeNavigator<XNode, IXNodeNavigator<TNode>> GetChild(int index);
    }

    public interface IXNodeNavigator<out TNode, out TParent> : IXNodeNavigator<TNode>, IFluentReturn<TParent>
    {
        new IXNodeNavigator<TNodeType, IXNodeNavigator<TNode, TParent>> To<TNodeType>()
            where TNodeType : XNode;

        new IXNodeNavigator<XNode, IXNodeNavigator<TNode, TParent>> GetChild(int index);
    }
}
