using System;

namespace Lux.Serialization
{
    public interface IDeserializer
    {
        object Deserialize(object input, Type type);
        T Deserialize<T>(object input);
    }
}
