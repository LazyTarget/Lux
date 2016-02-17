using System;
using System.Collections.Generic;
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


        
        public static IXNodeInterpreterIterator<XNode, TInterpreter> ChildrenOfType<TInterpreter>(this TInterpreter interpreter, Type nodeType)
            where TInterpreter : IXNodeInterpreter
        {
            var enumerable = interpreter.Children().EnumerateByNodeType(nodeType);
            
            var intepreters = enumerable.Select(x =>
            {
                var node = (XNode) x.GetNode();
                var res = XNodeInterpreter<XNode, TInterpreter>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<XNode, TInterpreter>(intepreters, interpreter);
            return iterator;
        }

        public static IXNodeInterpreterIterator<TNode, TInterpreter> ChildrenOfType<TInterpreter, TNode>(this TInterpreter interpreter)
            where TInterpreter : IXNodeInterpreter
            where TNode : XNode
        {
            var enumerable = interpreter.Children().EnumerateByNodeType<TNode>();

            var intepreters = enumerable.Select(x =>
            {
                var node = (TNode)x.GetNode();
                var res = XNodeInterpreter<TNode, TInterpreter>.Create(node, interpreter);
                return res;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNode, TInterpreter>(intepreters, interpreter);
            return iterator;
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




        public static IXNodeInterpreterIterator<XElement, TInterpreter> ChildrenWihTag<TInterpreter>(this TInterpreter interpreter, XName tagName)
            where TInterpreter : IXNodeInterpreter
        {
            var enumerable = interpreter.ChildrenOfType<TInterpreter, XElement>().Enumerate();
            
            var filtered = enumerable.Where(x =>
            {
                var node = x.GetNode();
                var res = node.Name == tagName;
                return res;
            });
            var selection = filtered.Select(x =>
            {
                var node = x.GetNode();
                var navigator = XNodeInterpreter<XElement, TInterpreter>.Create(node, interpreter);
                return navigator;
            });

            var intepreters = selection.ToArray();
            var iterator = new XNodeInterpreterIterator<XElement, TInterpreter>(intepreters, interpreter);
            return iterator;
        }


        
        public static IEnumerable<IXNodeInterpreter<TNodeType>> OfNodeType<TInterpreter, TNodeType>(this IEnumerable<TInterpreter> enumerable)
            where TInterpreter : IXNodeInterpreter
            where TNodeType : XNode
        {
            var filtered = enumerable.Where(x =>
            {
                var node = x.GetNode();
                var res = node is TNodeType;
                return res;
            });
            var selection = filtered.Select(x =>
            {
                TNodeType node = (TNodeType) x.GetNode();
                var interpreter = XNodeInterpreter<TNodeType, IXNodeInterpreter>.Create(node);
                return interpreter;
            });
            return selection;
        }

        public static IEnumerable<IXNodeInterpreter<TNodeType, TParent>> OfNodeType<TNode, TParent, TNodeType>(this IEnumerable<IXNodeInterpreter<TNode, TParent>> enumerable)
            where TNode : XNode
            where TNodeType : XNode
        {
            var filtered = enumerable.Where(x =>
            {
                var node = x.GetNode();
                var res = node is TNodeType;
                return res;
            });
            var selection = filtered.Select(x =>
            {
                TNodeType node = (TNodeType) (object) x.GetNode();
                TParent parent = x.Return();
                var interpreter = XNodeInterpreter<TNodeType, TParent>.Create(node, parent);
                return interpreter;
            });
            return selection;
        }


        public static IEnumerable<IXNodeInterpreter<XElement>> FilterByTagName<TInterpreter>(this IEnumerable<TInterpreter> enumerable, XName tagName)
            where TInterpreter : IXNodeInterpreter
        {
            var elems = enumerable.OfNodeType<TInterpreter, XElement>();

            var filtered = elems.Where(x =>
            {
                var node = x.GetNode();
                var res = node.Name == tagName;
                return res;
            });
            return filtered;
        }

        public static IEnumerable<IXNodeInterpreter<XElement, TParent>> FilterByTagName<TNode, TParent>(this IEnumerable<IXNodeInterpreter<TNode, TParent>> enumerable, XName tagName)
            where TNode : XNode
        {
            var elems = enumerable.OfNodeType<TNode, TParent, XElement>();

            var filtered = elems.Where(x =>
            {
                var node = x.GetNode();
                var res = node.Name == tagName;
                return res;
            });
            return filtered;
        }


    }
}
