using System;
using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeInterpreterBuilderExtensions
    {
        public static IFluentXElementBuilder<TNodeType, IXNodeInterpreter> BuildAndAppend<TNodeType>(this IXNodeInterpreter interpreter)
            where TNodeType : XElement, new()
        {
            var builder = new FluentXElementBuilder<TNodeType, IXNodeInterpreter>(interpreter);
            builder.OnCreate = (result) =>
            {
                var node = interpreter.GetNode();
                var container = (XContainer)node;
                container.Add(result);
            };
            return builder;
        }

        public static IFluentXElementBuilder<TNodeType, IXNodeInterpreter<TNode>> BuildAndAppend<TNodeType, TNode>(this IXNodeInterpreter<TNode> interpreter)
            where TNodeType : XElement, new()
            where TNode : XElement
        {
            var builder = new FluentXElementBuilder<TNodeType, IXNodeInterpreter<TNode>>(interpreter);
            builder.OnCreate = (result) =>
            {
                var node = interpreter.GetNode();
                node.Add(result);
            };
            return builder;
        }

        public static IFluentXElementBuilder<TNodeType, IXNodeInterpreter<TNode, TParent>> BuildAndAppend<TNodeType, TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter)
            where TNodeType : XElement, new()
            where TNode : XElement
        {
            var builder = new FluentXElementBuilder<TNodeType, IXNodeInterpreter<TNode, TParent>>(interpreter);
            builder.OnCreate = (result) =>
            {
                var node = interpreter.GetNode();
                node.Add(result);
            };
            return builder;
        }

    }
}
