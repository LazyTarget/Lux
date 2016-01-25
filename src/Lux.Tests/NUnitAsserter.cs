using Lux.Unittest;
using NUnit.Framework;

namespace Lux.Tests
{
    public class NUnitAsserter : IAsserter
    {
        public void AreEqual(object expected, object actual, string errorMessage)
        {
            Assert.AreEqual(expected, actual, errorMessage);
        }

        public void IsTrue(bool condition, string errorMessage)
        {
            Assert.IsTrue(condition, errorMessage);
        }

        public void Fail(string errorMessage)
        {
            Assert.Fail(errorMessage);
        }
    }
}
