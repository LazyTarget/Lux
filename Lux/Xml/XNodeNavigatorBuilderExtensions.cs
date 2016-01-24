using System;
using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeNavigatorBuilderExtensions
    {
        public static IFluentXElementBuilder<TNode, IXNodeNavigator<TNode>> BuildAndAppend<TNode>(this IXNodeNavigator<TNode> navigator)
            where TNode : XElement, new()
        {
            var builder = new FluentXElementBuilder<TNode, IXNodeNavigator<TNode>>(navigator);
            builder.OnCreate = (result) =>
            {
                var node = navigator.GetNode();
                node.Add(result);
            };
            return builder;
        }

    }
}
