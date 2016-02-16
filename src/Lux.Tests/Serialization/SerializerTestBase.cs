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

        
        [TestCase]
        public void Test()
        {
            var sut = GetSUT();

            var obj = new
            {
                StringProp = "Qwerty",
                DoubleProp = 13.1,
                IntProp = 5,
            };
            var xml = sut.Serialize(obj);

            var node = XElement.Parse(xml);
            node.CreateInterpreter()
                .AssertHasChildren(2)
                .GetChild(0)
                    .AssertHasChildren(0)
                    .AssertTagName(nameof(obj.StringProp))
                    .AssertElementValue(obj.StringProp)
                    //.AssertAttributeValue(nameof(obj.StringProp), obj.StringProp)
                    .Return()
                .GetChild(1)
                    .AssertHasChildren(0)
                    .AssertTagName(nameof(obj.DoubleProp))
                    .AssertElementValue(obj.DoubleProp)
                    //.AssertAttributeValue(nameof(obj.DoubleProp), obj.DoubleProp)
                    .Return()
                .GetChild(2)
                    .AssertHasChildren(0)
                    .AssertTagName(nameof(obj.IntProp))
                    .AssertElementValue(obj.IntProp)
                    //.AssertAttributeValue(nameof(obj.IntProp), obj.IntProp)
                    .Return()
                ;
        }
        
    }
}
