using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeInterpreterExtensions
    {
        public static IXNodeInterpreter<TNode> CreateInterpreter<TNode>(this TNode node)
            where TNode : XNode
        {
            var navigator = XNodeInterpreter.Create(node);
            return navigator;
        }

        public static IXNodeInterpreter<TNode> GetChildAs<TNode>(this IXNodeInterpreter interpreter, int index)
            where TNode : XNode
        {
            var child = interpreter.GetChild(index);
            var result = child.To<TNode>();
            return result;
        }

    }
}
