using System;

namespace Lux.Unittest
{
    public class DotNetAsserter : IAsserter
    {
        public void AreEqual(object expected, object actual, string errorMessage)
        {
            var res = Object.Equals(expected, actual);
            if (!res)
                Fail($"Objects are not equal. Expected: {expected}, Actual: {actual}");
        }

        public void IsTrue(bool condition, string errorMessage)
        {
            if (!condition)
                Fail($"Condition is not true");
        }

        public void Fail(string errorMessage)
        {
            if (errorMessage == null)
                errorMessage = "Assertion exception";
            throw new Exception(errorMessage);
        }
    }
}
