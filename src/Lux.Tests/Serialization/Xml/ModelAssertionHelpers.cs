using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Lux.Tests.Serialization.Models;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Serialization.Xml
{
    public static class ModelAssertionHelpers
    {
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
                throw new AssertionException($"Missing property element '{nameof(PocoClass.PocoProp)}'");
            }

            return interpreter;
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
