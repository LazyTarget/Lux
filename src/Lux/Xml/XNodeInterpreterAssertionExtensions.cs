using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Lux.Unittest;

namespace Lux.Xml
{
    public static class XNodeInterpreterAssertionExtensions
    {
        private static IAsserter Assert = AssertConfig.Asserter;



        public static IXNodeInterpreter<TNode> AssertHasChildren<TNode>(this IXNodeInterpreter<TNode> interpreter, int? count = null)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var container = (XContainer) (object) node;
            var children = container.Nodes().ToList();
            if (count.HasValue)
                Assert.AreEqual(count.Value, children.Count, "Tag children count not equal to expectation");
            else
                Assert.IsTrue(children.Count > 0, "Tag has no children");
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertHasAttribute<TNode>(this IXNodeInterpreter<TNode> interpreter, string attributeName)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var elem = (XElement)(object)node;

            var attr = elem.Attribute(attributeName);
            if (attr == null)
            {
                Assert.Fail($"Element doesn't have attribute '{attributeName}'");
            }
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertAttributeValue<TNode>(this IXNodeInterpreter<TNode> interpreter, string attributeName, object attributeValue)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
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
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertPropertyValue<TNode>(this IXNodeInterpreter<TNode> interpreter, Expression<Func<TNode, object>> propertyLambda, object value)
            where TNode : XNode
        {
            var propertyInfo = propertyLambda.GetPropertyFromExpression();
            if (propertyInfo != null)
            {
                var node = interpreter.GetNode();
                var currentValue = propertyInfo.GetValue(node);
                Assert.AreEqual(value, currentValue, "Property values are not equal");
            }
            else
                throw new InvalidOperationException("Invalid property");
            return interpreter;
        }

    }
}
