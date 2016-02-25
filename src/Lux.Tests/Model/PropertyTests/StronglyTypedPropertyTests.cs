using System;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public class StronglyTypedPropertyTests : PropertyTestBase
    {
        protected override IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false)
        {
            var property = new StronglyTypedProperty(name, type, isReadOnly, value);
            return property;
        }





        [TestCase]
        public virtual void AllowsValueWhenSameType()
        {
            var expected = Fixture.Create<BaseClass>();
            var property = CreateProperty<BaseClass>(name: Fixture.Create<string>(), value: expected);
            var actual = property.Value;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public virtual void AllowsValueWhenDerivedInterfaceType()
        {
            var expected = Fixture.Create<BaseClass>();
            var property = CreateProperty<IBaseClass>(name: Fixture.Create<string>(), value: expected);
            var actual = property.Value;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public virtual void AllowsValueWhenDerivedClassType()
        {
            var expected = Fixture.Create<DerivedClass>();
            var property = CreateProperty<BaseClass>(name: Fixture.Create<string>(), value: expected);
            var actual = property.Value;
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public virtual void ThrowsExceptionWhenWrongType()
        {
            var value = Fixture.Create<string>();
            TestDelegate act = () => CreateProperty<int>(name: Fixture.Create<string>(), value: value);
            Assert.Throws<InvalidOperationException>(act);
        }

    }
}
