using Lux.Serialization;
using Lux.Serialization.Xml;
using NUnit.Framework;

namespace Lux.Tests.Serialization.Xml
{
    [TestFixture]
    public class XmlSerializerTests : SerializerTestBase
    {
        protected override ISerializer GetSUT()
        {
            return new XmlSerializer();
            //return new DotNetXmlSerializer();
        }
        
    }
}
