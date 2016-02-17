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
        public virtual void SerializePoco()
        {
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
                .AssertHasChildren(3)
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



        public class PocoClass
        {
            public string StringProp { get; set; }
            public double DoubleProp { get; set; }
            public int IntProp { get; set; }
            public object ObjectProp { get; set; }
        }

    }
}
