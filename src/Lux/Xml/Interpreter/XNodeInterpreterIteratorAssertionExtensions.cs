using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Unittest;

namespace Lux.Xml
{
    public static class XNodeInterpreterIteratorAssertionExtensions
    {
        private static IAsserter Assert = Framework.Asserter;


        public static IXNodeInterpreterIterator<TNode> AssertCount<TNode>(this IXNodeInterpreterIterator<TNode> iterator, int? count = null)
            where TNode : XNode
        {
            var enumerable = iterator.Enumerate();
            var actual = enumerable.Count();
            if (count.HasValue)
                Assert.AreEqual(count.Value, actual, "Tag children count not equal to expectation");
            else
                Assert.IsTrue(actual > 0, "Node has no children");
            return iterator;
        }

        public static IXNodeInterpreterIterator<TNode, TParent> AssertCount<TNode, TParent>(this IXNodeInterpreterIterator<TNode, TParent> iterator, int? count = null)
            where TNode : XNode
        {
            AssertCount((IXNodeInterpreterIterator<TNode>)iterator, count);
            return iterator;
        }
    }
}
