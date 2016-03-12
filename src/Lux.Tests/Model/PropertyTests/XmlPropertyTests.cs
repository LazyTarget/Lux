using System;
using System.Xml.Linq;
using Lux.Model;
using Lux.Model.Xml;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public class XmlPropertyTests : StronglyTypedPropertyTests
    {
        protected override IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false)
        {
            var element = new XElement("property");
            var property = new XElementPropertyParser.XmlProperty(element);
            property.Name = name;
            property.Type = type;
            property.ReadOnly = isReadOnly;
            property.SetValue(value);
            return property;
        }


        [TestCase, Ignore("Test not applicable")]
        public override void WhenNoTypeConstraint()
        {
            Assert.Inconclusive("Test not applicable");
        }


        [TestCase]
        public virtual void ThrowsExceptionWhenWrongTypeOfT()
        {
            var value = Fixture.Create<string>();
            TestDelegate act = () => CreateProperty<int>(name: Fixture.Create<string>(), value: value);
            Assert.Throws<InvalidOperationException>(act);
        }

    }
}
