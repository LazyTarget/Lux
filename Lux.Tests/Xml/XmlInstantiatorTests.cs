using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Xml
{
    [TestFixture]
    public class XmlInstantiatorTests
    {
        private XmlInstantiator GetSUT()
        {
            return new XmlInstantiator();
        }


        [TestCase]
        public void InstantiateElement_CanInstantiateValue_WhenString()
        {
            var sut = GetSUT();

            var expected = "Some text";
            var elem = new XElement("tag");
            elem.SetValue(expected);

            var actual = sut.InstantiateElement(elem);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void InstantiateElement_CanInstantiateValue_WhenInteger()
        {
            var sut = GetSUT();

            var expected = 13;
            var elem = new XElement("tag");
            elem.SetValue(expected);

            var actual = sut.InstantiateElement(elem);
            Assert.AreEqual(expected.ToString(), actual);
        }

    }
}
