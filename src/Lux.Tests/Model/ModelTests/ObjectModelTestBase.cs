using System;
using System.Linq;
using System.Reflection;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public abstract class ObjectModelTestBase : ModelTestBase
    {
        protected abstract IObjectModel CreateObjectModel(object args);


        [TestCase]
        public virtual void SetPropertyValues()
        {
            var instance = Fixture.Create<PocoClass>();
            var objectModel = CreateObjectModel(instance);

            var properties = objectModel.GetProperties().ToList();
            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    if (property.ReadOnly)
                        continue;

                    var type = property.Type ?? typeof (object);
                    var specimenContext = new SpecimenContext(Fixture);
                    var newValue = specimenContext.Resolve(type);
                    property.SetValue(newValue);
                    Assert.AreEqual(newValue, property.Value);
                }
            }
            else
            {
                Assert.Inconclusive("ObjectModel has no properties, cannot test if values are set");
            }
        }


        [TestCase]
        public virtual void VerifyPropertyValues()
        {
            var instance = Fixture.Create<BaseClass>();
            var properties = instance.GetType().GetProperties();
            if (properties.Length <= 0)
                Assert.Fail("Class has no properties");

            var objectModel = CreateObjectModel(instance);

            foreach (var propertyInfo in properties)
            {
                var prop = objectModel.GetProperty(propertyInfo.Name);
                Assert.IsNotNull(prop, "Property '{0}' is not defined", propertyInfo.Name);
                var expected = propertyInfo.GetValue(instance);
                var actual = prop.Value;
                Assert.AreEqual(expected, actual);
            }
        }

    }
}
