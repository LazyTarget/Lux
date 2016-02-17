using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Lux.Tests.Serialization.Models;
using Lux.Unittest;
using Lux.Xml;

namespace Lux.Tests.Serialization.Xml
{
    public static class ModelAssertionHelpers
    {
        public static IAsserter Assert = Framework.Asserter;


        public static TInterpreter AssertAreEquivalent<TInterpreter>(this TInterpreter interpreter, PocoClass expected, CultureInfo cultureInfo = null)
            where TInterpreter : IXNodeInterpreter
        {
            var iterator = interpreter.ChildrenOfType(typeof (XElement));

            if (expected == null)
            {
                // todo: either the property element is missing or has an empty value

                var enumerable = iterator.Enumerate().ToList();
                if (!enumerable.Any())
                {
                    return interpreter;
                }

                iterator
                    .AssertCount(0, nameof(PocoClass.StringProp))
                    .AssertCount(0, nameof(PocoClass.DoubleProp))
                    .AssertCount(0, nameof(PocoClass.IntProp))
                    .AssertCount(0, nameof(PocoClass.PocoProp));
                
                return interpreter;
            }

            iterator
                .AssertProperty(nameof(PocoClass.StringProp),   expected.StringProp.ToString(cultureInfo))
                .AssertProperty(nameof(PocoClass.DoubleProp),   expected.DoubleProp.ToString(cultureInfo))
                .AssertProperty(nameof(PocoClass.IntProp),      expected.IntProp.ToString(cultureInfo));
            
            var pocoProp = interpreter.ChildrenWihTag(nameof(PocoClass.PocoProp)).Enumerate().FirstOrDefault();
            if (pocoProp != null)
            {
                AssertAreEquivalent(pocoProp, expected.PocoProp, cultureInfo);
            }
            else if (expected.PocoProp != null)
            {
                Assert.Fail($"Missing property element '{nameof(PocoClass.PocoProp)}'");
            }

            
            var pocoList = interpreter.ChildrenWihTag(nameof(PocoClass.PocoList)).Enumerate().FirstOrDefault();
            if (pocoList != null)
            {
                AssertAreEquivalent(pocoList, expected.PocoList, cultureInfo);
            }
            else if (expected.PocoList != null)
            {
                Assert.Fail($"Missing property list '{nameof(PocoClass.PocoList)}'");
            }

            return interpreter;
        }


        public static TInterpreter AssertAreEquivalent<TInterpreter>(this TInterpreter interpreter, IList<PocoClass> expected, CultureInfo cultureInfo = null)
            where TInterpreter : IXNodeInterpreter
        {
            var iterator = interpreter.ChildrenOfType(typeof (XElement));

            if (expected == null)
            {
                // todo: either the property element is missing or has an empty value

                var enumerable = iterator.Enumerate().ToList();
                if (!enumerable.Any())
                {
                    return interpreter;
                }
                Assert.Fail("Expected null");
                return interpreter;
            }


            iterator.AssertCount(expected.Count);

            var elems = iterator.Enumerate().ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                var elem = elems.ElementAt(i);
                var poco = expected.ElementAt(i);
                AssertAreEquivalent(elem, poco, cultureInfo);
            }
            return interpreter;
        }

    }
}
