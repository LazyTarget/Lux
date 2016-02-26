using System;
using System.Linq;
using Lux.Extensions;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public abstract class ModelTestBase : TestBase
    {
        protected override IFixture CreateFixture()
        {
            var fixture = base.CreateFixture();
            fixture.Behaviors.RemoveAll(x => x is ThrowingRecursionBehavior);
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(3));

            fixture.Customize(new FreezingCustomization(typeof (DerivedClass), typeof (object)));
            return fixture;
        }
    }
}
