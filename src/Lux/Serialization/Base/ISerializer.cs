using System.Globalization;

namespace Lux.Serialization
{
    public interface ISerializer
    {
        CultureInfo Culture { get; set; }

        string Serialize(object obj);
    }
}
