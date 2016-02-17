using System;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using Lux.Extensions;
using Lux.Unittest;

namespace Lux.Xml
{
    public static class XNodeInterpreterAssertionExtensions
    {
        private static IAsserter Assert = Framework.Asserter;



        public static IXNodeInterpreter<TNode> AssertHasChildren<TNode>(this IXNodeInterpreter<TNode> interpreter, int? count = null)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var container = (XContainer)(object)node;
            var children = container.Nodes().ToList();
            if (count.HasValue)
                Assert.AreEqual(count.Value, children.Count, "Tag children count not equal to expectation");
            else
                Assert.IsTrue(children.Count > 0, "Node has no children");
            return interpreter;
        }

        public static IXNodeInterpreter<TNode, TParent> AssertHasChildren<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, int? count = null)
            where TNode : XNode
        {
            AssertHasChildren((IXNodeInterpreter<TNode>) interpreter, count);
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

        public static IXNodeInterpreter<TNode, TParent> AssertHasAttribute<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, string attributeName)
            where TNode : XNode
        {
            AssertHasAttribute((IXNodeInterpreter<TNode>)interpreter, attributeName);
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertAttributeValue<TNode>(this IXNodeInterpreter<TNode> interpreter, string attributeName, object attributeValue)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var elem = (XElement)(object)node;

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

        public static IXNodeInterpreter<TNode, TParent> AssertAttributeValue<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, string attributeName, object attributeValue)
            where TNode : XNode
        {
            AssertAttributeValue((IXNodeInterpreter<TNode>)interpreter, attributeName, attributeValue);
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertElementValue<TNode>(this IXNodeInterpreter<TNode> interpreter, object elementValue)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var elem = (XElement)(object)node;
            
            var value = elem.Value;
            Assert.AreEqual(elementValue, value, $"Element values don't match");
            return interpreter;
        }

        public static IXNodeInterpreter<TNode, TParent> AssertElementValue<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, object elementValue)
            where TNode : XNode
        {
            AssertElementValue((IXNodeInterpreter<TNode>)interpreter, elementValue);
            return interpreter;
        }


        public static IXNodeInterpreter<TNode> AssertTagName<TNode>(this IXNodeInterpreter<TNode> interpreter, object tagName)
            where TNode : XNode
        {
            var node = interpreter.GetNode();
            var elem = (XElement)(object)node;
            
            var value = elem.Name;
            Assert.AreEqual(tagName, value, $"Tag names don't match");
            return interpreter;
        }

        public static IXNodeInterpreter<TNode, TParent> AssertTagName<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, object tagName)
            where TNode : XNode
        {
            AssertTagName((IXNodeInterpreter<TNode>)interpreter, tagName);
            return interpreter;
        }



        public static IXNodeInterpreter<TNode> AssertPropertyValue<TNode>(this IXNodeInterpreter<TNode> interpreter, Expression<Func<TNode, object>> propertyLambda, object value)
            where TNode : XNode
        {
            var propertyInfo = propertyLambda.GetPropertyInfoByExpression();
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

        public static IXNodeInterpreter<TNode, TParent> AssertPropertyValue<TNode, TParent>(this IXNodeInterpreter<TNode, TParent> interpreter, Expression<Func<TNode, object>> propertyLambda, object value)
            where TNode : XNode
        {
            AssertPropertyValue((IXNodeInterpreter<TNode>)interpreter, propertyLambda, value);
            return interpreter;
        }

    }
}
