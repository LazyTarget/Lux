using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using Lux.Serialization;
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
                //.Children()
                .ChildrenOfType(typeof(XElement))
                    .AssertCount(3)
                    .GetAtIndex(0)
                        .AssertTagName(nameof(obj.StringProp))
                        .AssertElementValue(obj.StringProp.ToString(cultureInfo))
                        .ChildrenOfType(typeof(XElement))
                            .AssertCount(0)
                            .Return()
                        .Return()
                    .GetAtIndex(1)
                        .AssertTagName(nameof(obj.DoubleProp))
                        .AssertElementValue(obj.DoubleProp.ToString(cultureInfo))
                        .ChildrenOfType(typeof(XContainer))
                            .AssertCount(0)
                            .Return()
                        .Return()
                    .GetAtIndex(2)
                        .AssertTagName(nameof(obj.IntProp))
                        .AssertElementValue(obj.IntProp.ToString(cultureInfo))
                        .ChildrenOfType(typeof(XContainer))
                            .AssertCount(0)
                            .Return()
                        .Return()
                    .Return()
                ;
        }



        public class PocoClass
        {
            public string StringProp { get; set; }
            public double DoubleProp { get; set; }
            public int IntProp { get; set; }
            public object ObjectProp { get; set; }
        }

    }
}
