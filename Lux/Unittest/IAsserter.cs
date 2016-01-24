namespace Lux.Unittest
{
    public interface IAsserter
    {
        void AreEqual(object expected, object actual, string errorMessage);

        void IsTrue(bool condition, string errorMessage);

        void Fail(string errorMessage);
    }
}
