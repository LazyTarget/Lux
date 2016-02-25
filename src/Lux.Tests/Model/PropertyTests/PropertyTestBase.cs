using System;
using Lux.Model;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public abstract class PropertyTestBase : TestBase
    {
        protected virtual IProperty CreateProperty<TValue>(string name, object value = null, bool isReadOnly = false)
        {
            var property = CreateProperty(name, typeof (TValue), value, isReadOnly);
            return property;
        }

        protected abstract IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false);



        [TestCase]
        public virtual void ReturnsPropertyName()
        {
            var expected = Fixture.Create<string>();
            var property = CreateProperty(name: expected);
            var actual = property.Name;
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public virtual void ReturnsTheInitialValue()
        {
            var expected = Fixture.Create<string>();
            var property = CreateProperty(name: Fixture.Create<string>(), value: expected);
            var actual = property.Value;
            Assert.AreEqual(expected, actual);
        }

    }
}
