using System.IO;

namespace Lux.Config
{
    public interface IConfigLocation
    {
        bool CanRead { get; }
        bool CanWrite { get; }

        Stream GetStreamForRead(IConfigArguments arguments);
        Stream GetStreamForWrite(IConfigArguments arguments);
    }
}
