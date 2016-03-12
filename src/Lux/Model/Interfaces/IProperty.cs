using System;

namespace Lux.Model
{
    public interface IProperty
    {
        string Name { get; }
        Type Type { get; }
        bool ReadOnly { get; }
        object Value { get; }
        void SetValue(object value);
    }
}
