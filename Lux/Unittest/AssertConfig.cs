using System;

namespace Lux.Unittest
{
    public static class AssertConfig
    {
        private static IAsserter _asserter = new EmptyAsserter();


        public static IAsserter Asserter
        {
            get { return _asserter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _asserter = value;
            }
        }

    }
}
