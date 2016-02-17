using System;
using System.Globalization;
using System.Xml.Linq;
using Lux.Serialization;
using Lux.Tests.Serialization.Models;
using Lux.Tests.Serialization.Xml;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Serialization
{
    [TestFixture]
    public abstract class SerializerTestBase : TestBase
    {
        protected abstract ISerializer GetSUT();

        protected abstract CultureInfo GetCultureInfo();

        
        [TestCase]
        public virtual void SerializePoco()
        {
            var cultureInfo = GetCultureInfo();
            var sut = GetSUT();

            var obj = new PocoClass
            {
                StringProp = "Qwerty",
                DoubleProp = 13.1,
                IntProp = 5,
            };
            var xml = sut.Serialize(obj);

            var node = XElement.Parse(xml);
            node.CreateInterpreter()
                .AssertTagName(nameof(PocoClass))
                .AssertAreEquivalent(obj, cultureInfo);
        }


        [TestCase]
        public virtual void SerializePoco_WithNesting()
        {
            var cultureInfo = GetCultureInfo();
            var sut = GetSUT();

            var obj = new PocoClass
            {
                StringProp = "Qwerty",
                DoubleProp = 13.1,
                IntProp = 5,
                PocoProp = new PocoClass
                {
                    StringProp = "Foo",
                    DoubleProp = 1.052,
                    IntProp = 421,
                    PocoProp = new PocoClass
                    {
                        StringProp = "Bar"
                    },
                },
            };
            var xml = sut.Serialize(obj);

            var node = XElement.Parse(xml);
            node.CreateInterpreter()
                .AssertTagName(nameof(PocoClass))
                .AssertAreEquivalent(obj, cultureInfo);
        }

    }
}
