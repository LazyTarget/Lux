using System;

namespace Lux.Serialization
{
    public interface IProperty
    {
        string Name { get; }
        Type Type { get; }
        object Value { get; }
        void SetValue(object value);
    }
}
