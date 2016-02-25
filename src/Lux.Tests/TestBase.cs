using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected IFixture Fixture { get; private set; }


        [SetUp]
        protected void GlobalSetUp()
        {
            Fixture = CreateFixture();

            Framework.Asserter = new NUnitAsserter();

            SetUp();
        }

        protected virtual void SetUp()
        {

        }

        [TearDown]
        protected void GlobalTearDown()
        {
            TearDown();
        }

        protected virtual void TearDown()
        {

        }


        protected IFixture CreateFixture()
        {
            var fixture = new Fixture();
            return fixture;
        }


    }
}
