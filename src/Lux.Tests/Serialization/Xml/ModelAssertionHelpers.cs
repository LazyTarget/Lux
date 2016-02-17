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
                .AssertCount(1, nameof(PocoClass.StringProp))
                .GetByTagName(nameof(PocoClass.StringProp))
                    .AssertElementValue(expected.StringProp.ToString(cultureInfo))
                    .ChildrenOfType(typeof (XElement))
                        .AssertCount(0)
                    .Return()
                .Return()

                .AssertCount(1, nameof(PocoClass.DoubleProp))
                .GetByTagName(nameof(PocoClass.DoubleProp))
                    .AssertTagName(nameof(PocoClass.DoubleProp))
                    .AssertElementValue(expected.DoubleProp.ToString(cultureInfo))
                    .ChildrenOfType(typeof (XContainer))
                        .AssertCount(0)
                    .Return()
                .Return()

                .AssertCount(1, nameof(PocoClass.IntProp))
                .GetByTagName(nameof(PocoClass.IntProp))
                    .AssertTagName(nameof(PocoClass.IntProp))
                    .AssertElementValue(expected.IntProp.ToString(cultureInfo))
                    .ChildrenOfType(typeof (XContainer))
                        .AssertCount(0)
                    .Return()
                .Return();


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

    }
}
