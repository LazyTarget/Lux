namespace Lux.Unittest
{
    public class EmptyAsserter : IAsserter
    {
        public void AreEqual(object expected, object actual, string errorMessage)
        {
        }

        public void IsTrue(bool condition, string errorMessage)
        {
        }

        public void Fail(string errorMessage)
        {
        }
    }
}
