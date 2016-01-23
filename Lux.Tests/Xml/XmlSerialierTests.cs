using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Serialization;
using Lux.Serialization.Xml;
using NUnit.Framework;

namespace Lux.Tests.Xml
{
    [TestFixture]
    public class XmlSerialierTests
    {
        private IXmlSerializer GetSUT()
        {
            return new XmlSerializer();
        }


        [TestCase]
        public void SerializeXml()
        {
            var sut = GetSUT();

            var expected = "Some text";
            var propName = "propName";
            
            var node = new XmlDocument();
            node.SetPropertyValue(propName, expected);

            var xdoc = sut.Serialize(node);
            Assert.IsNotNull(xdoc);

            var elem = xdoc.Root;
            var propElem = elem?.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == propName);
            var actual = propElem?.GetAttributeValue("value") ?? propElem?.Value;
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void DeserializeXml()
        {
            var sut = GetSUT();

            var expected = "Some text";
            var propName = "propName";

            var elem = new XElement("tag");
            var propElem = elem.GetOrCreateElement("property", x => x.GetAttributeValue("name") == propName);
            propElem.SetAttributeValue("name", propName);
            propElem.SetAttributeValue("value", expected);


            var obj = sut.Deserialize<XmlObject>(elem);
            Assert.IsNotNull(obj);

            var actual = obj.GetPropertyValue(propName);
            Assert.AreEqual(expected, actual);
        }

    }
}
