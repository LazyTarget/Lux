using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeInterpreterIteratorExtensions
    {
        public static IXNodeInterpreter<XElement, IXNodeInterpreterIterator<TNode, TParent>> GetByTagName<TNode, TParent>(this IXNodeInterpreterIterator<TNode, TParent> iterator, XName tagName, int index = 0)
            where TNode : XNode
        {
            var enumerable = iterator.Enumerate().FilterByTagName(tagName);
            var navigator = enumerable.ElementAt(index);
            var node = navigator.GetNode();
            var result = XNodeInterpreter<XElement, IXNodeInterpreterIterator<TNode, TParent>>.Create(node, iterator);
            return result;
        }


        public static IEnumerable<IXNodeInterpreter<XNode, IXNodeInterpreterIterator>> EnumerateByNodeType(this IXNodeInterpreterIterator iterator, Type nodeType)
        {
            var enumerable = iterator.Enumerate();
            var filtered = enumerable.Where(interpreter =>
            {
                var node = interpreter.GetNode();
                var res = nodeType.IsInstanceOfType(node);
                return res;
            });
            var selection = filtered.Select(interpreter =>
            {
                var node = interpreter.GetNode();
                var navigator = XNodeInterpreter<XNode, IXNodeInterpreterIterator>.Create(node, iterator);
                return navigator;
            });
            return selection;
        }

        public static IEnumerable<IXNodeInterpreter<TNode, IXNodeInterpreterIterator>> EnumerateByNodeType<TNode>(this IXNodeInterpreterIterator iterator)
            where TNode : XNode
        {
            var enumerable = iterator.Enumerate();
            var filtered = enumerable.Where(x =>
            {
                var node = x.GetNode();
                var res = node is TNode;
                return res;
            });
            var selection = filtered.Select(x =>
            {
                var node = (TNode) x.GetNode();
                var navigator = XNodeInterpreter<TNode, IXNodeInterpreterIterator>.Create(node, iterator);
                return navigator;
            });
            return selection;
        }

    }
}
