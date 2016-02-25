using System;
using Lux.Model;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public class StronglyTypedPropertyOfTTests : StronglyTypedPropertyTests
    {
        protected override IProperty CreateProperty<TValue>(string name, object value = null, bool isReadOnly = false )
        {
            var property = new StronglyTypedProperty<TValue>(name, isReadOnly, value);
            return property;
        }

        protected override IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false)
        {
            var property = CreateProperty<object>(name: name, value: value, isReadOnly: isReadOnly);
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
