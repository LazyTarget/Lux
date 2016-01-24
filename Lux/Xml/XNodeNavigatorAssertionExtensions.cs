using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Lux.Unittest;

namespace Lux.Xml
{
    public static class XNodeNavigatorAssertionExtensions
    {
        private static IAsserter Assert = AssertConfig.Asserter;



        public static IXNodeNavigator<TNode> AssertHasChildren<TNode>(this IXNodeNavigator<TNode> navigator, int? count = null)
            where TNode : XNode
        {
            var node = navigator.GetNode();
            var container = (XContainer) (object) node;
            var children = container.Nodes().ToList();
            if (count.HasValue)
                Assert.AreEqual(count.Value, children.Count, "Tag children count not equal to expectation");
            else
                Assert.IsTrue(children.Count > 0, "Tag has no children");
            return navigator;
        }


        public static IXNodeNavigator<TNode> AssertHasAttribute<TNode>(this IXNodeNavigator<TNode> navigator, string attributeName)
            where TNode : XNode
        {
            var node = navigator.GetNode();
            var elem = (XElement)(object)node;

            var attr = elem.Attribute(attributeName);
            if (attr == null)
            {
                Assert.Fail($"Element doesn't have attribute '{attributeName}'");
            }
            return navigator;
        }


        public static IXNodeNavigator<TNode> AssertAttributeValue<TNode>(this IXNodeNavigator<TNode> navigator, string attributeName, object attributeValue)
            where TNode : XNode
        {
            var node = navigator.GetNode();
            var elem = (XElement) (object) node;

            var attr = elem.Attribute(attributeName);
            if (attr != null)
            {
                var value = attr.Value;
                Assert.AreEqual(attributeValue, value, $"Attribute values don't match");
            }
            else
            {
                Assert.Fail($"Element doesn't have attribute '{attributeName}'");
            }
            return navigator;
        }


        public static IXNodeNavigator<TNode> AssertPropertyValue<TNode>(this IXNodeNavigator<TNode> navigator, Expression<Func<TNode, object>> propertyLambda, object value)
            where TNode : XNode
        {
            var propertyInfo = propertyLambda.GetPropertyFromExpression();
            if (propertyInfo != null)
            {
                var node = navigator.GetNode();
                var currentValue = propertyInfo.GetValue(node);
                Assert.AreEqual(value, currentValue, "Property values are not equal");
            }
            else
                throw new InvalidOperationException("Invalid property");
            return navigator;
        }

    }
}
