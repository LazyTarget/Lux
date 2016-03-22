using System;
using Lux.Model;
using Lux.Serialization.Json.JsonNet;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public class JsonPropertyTests : StronglyTypedPropertyTests
    {
        protected override IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false)
        {
            var obj = new JObject();
            var prop = new JProperty(name);
            obj.Add(prop);

            var property = new JObjectPropertyParser.JsonProperty(prop);
            //property.Name = name;
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
