using System;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public class MirrorObjectModelTests : ObjectModelTestBase
    {
        protected override IObjectModel CreateObjectModel(object args)
        {
            var objectModel = new MirrorObjectModel(args);
            return objectModel;
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
                var expected = propertyInfo.GetValue(instance);
                var actual = prop.Value;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
