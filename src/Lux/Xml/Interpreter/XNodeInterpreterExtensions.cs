using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeInterpreterExtensions
    {
        public static IXNodeInterpreter<TNode> CreateInterpreter<TNode>(this TNode node)
            where TNode : XNode
        {
            var interpreter = XNodeInterpreter.Create(node);
            return interpreter;
        }

        public static IXNodeInterpreter<TNode> GetChildAs<TNode>(this IXNodeInterpreter interpreter, int index)
            where TNode : XNode
        {
            var child = interpreter.GetChild(index);
            var result = child.To<TNode>();
            return result;
        }


        
        public static IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode>> ChildrenOfType<TNode>(this IXNodeInterpreter<TNode> interpreter, Type nodeType)
            where TNode : XNode
        {
            var enumerable = interpreter.Children().EnumerateByNodeType(nodeType);
            
            var intepreters = enumerable.Select(x =>
            {
                var node = (TNode) x.GetNode();
                var res = XNodeInterpreter<TNode, IXNodeInterpreter<TNode>>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode>>(intepreters, interpreter);
            return iterator;
        }



        public static IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>> ChildrenOfType<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, Type nodeType)
            where TNode : XNode
        {
            var enumerable = interpreter.Children().EnumerateByNodeType(nodeType);
            
            var intepreters = enumerable.Select(x =>
            {
                var node = (TNode) x.GetNode();
                var res = XNodeInterpreter<TNode, IXNodeInterpreter<TNode, TParent>>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>>(intepreters, interpreter);
            return iterator;
        }



        public static IXNodeInterpreterIterator<TNodeType, IXNodeInterpreter<TNode>> ChildrenOfType<TNode, TNodeType>(this IXNodeInterpreter<TNode> interpreter)
            where TNode : XNode
            where TNodeType : XNode
        {
            var enumerable = interpreter.Children().EnumerateByNodeType<TNodeType>();

            var intepreters = enumerable.Select(x =>
            {
                var node = (TNodeType)x.GetNode();
                var res = XNodeInterpreter<TNodeType, IXNodeInterpreter<TNode>>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNodeType, IXNodeInterpreter<TNode>>(intepreters, interpreter);
            return iterator;
        }

        public static IXNodeInterpreterIterator<TNodeType, IXNodeInterpreter<TNode, TParent>> ChildrenOfType<TNode, TParent, TNodeType>(this IXNodeInterpreter<TNode, TParent> interpreter)
            where TNode : XNode 
            where TNodeType : XNode
        {
            var enumerable = interpreter.Children().EnumerateByNodeType<TNodeType>();
            
            var intepreters = enumerable.Select(x =>
            {
                var node = (TNodeType) x.GetNode();
                var res = XNodeInterpreter<TNodeType, IXNodeInterpreter<TNode, TParent>>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNodeType, IXNodeInterpreter<TNode, TParent>>(intepreters, interpreter);
            return iterator;
        }

    }
}
