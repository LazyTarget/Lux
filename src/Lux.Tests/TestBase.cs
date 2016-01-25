using NUnit.Framework;

namespace Lux.Tests
{
    [TestFixture]
    public abstract class TestBase
    {
        [SetUp]
        protected void GlobalSetUp()
        {
            Lux.Unittest.AssertConfig.Asserter = new NUnitAsserter();

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


    }
}
