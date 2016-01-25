using Moq;

namespace Lux.IO
{
    public class DebugMock<T> : Mock<T>
        where T : class
    {
        public DebugMock(object referrer)
        {
            Referrer = referrer;
        }

        public object Referrer { get; private set; }


    }
}
