using System.Xml.Linq;

namespace Lux.Xml
{
    public static class XNodeNavigatorExtensions
    {
        public static IXNodeNavigator<TNode> GetChildAs<TNode>(this IXNodeNavigator navigator, int index)
            where TNode : XNode
        {
            var child = navigator.GetChild(index);
            var result = child.To<TNode>();
            return result;
        }

    }
}
