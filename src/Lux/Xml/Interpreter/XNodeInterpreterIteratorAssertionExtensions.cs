using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Unittest;

namespace Lux.Xml
{
    public static class XNodeInterpreterIteratorAssertionExtensions
    {
        public static IAsserter Assert = Framework.Asserter;


        public static TIterator AssertCount<TIterator>(this TIterator iterator, int? count = null, XName tagName = null)
            where TIterator : IXNodeInterpreterIterator
        {
            int actual;
            if (tagName != null)
            {
                //actual = iterator.EnumerateByNodeType(typeof (XElement))
                //                 .Count(x => ((XElement) x.GetNode()).Name == tagName);
                actual = iterator.Enumerate().FilterByTagName(tagName).Count();
            }
            else
                actual = iterator.Enumerate().Count();

            if (count.HasValue)
                Assert.AreEqual(count.Value, actual, "Tag children count not equal to expectation");
            else
                Assert.IsTrue(actual > 0, "Node has no children");
            return iterator;
        }


        public static TIterator AssertProperty<TIterator>(this TIterator iterator, string propertyName, string expected)
            where TIterator : IXNodeInterpreterIterator
        {
            iterator
                .AssertCount(1, propertyName)
                .GetByTagName(propertyName)
                    .AssertTagName(propertyName)
                    .AssertElementValue(expected)
                    .ChildrenOfType(typeof (XContainer))
                        .AssertCount(0)
                    .Return()
                .Return();

            return iterator;
        }

    }
}
