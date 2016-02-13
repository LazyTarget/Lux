using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Lux.Serialization.Xml;
using NUnit.Framework;

namespace Lux.Tests.Xml
{
    [TestFixture]
    public class XmlInstantiatorTests : TestBase
    {
        private IXmlInstantiator GetSUT()
        {
            return new CustomXmlSerializer();
        }


        [TestCase]
        public void InstantiateElement_CanInstantiateValue_WhenString()
        {
            var sut = GetSUT();

            var expected = "Some text";
            var elem = new XElement("tag");
            elem.SetValue(expected);

            var actual = sut.InstantiateFromElement(elem, null);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void InstantiateElement_CanInstantiateValue_WhenInteger()
        {
            var sut = GetSUT();

            var expected = 13;
            var elem = new XElement("tag");
            elem.SetValue(expected);

            var actual = sut.InstantiateFromElement(elem, null);
            Assert.AreEqual(expected.ToString(), actual);
        }

    }
}
