using System;

namespace Lux.Interfaces
{
    public interface IConverter
    {
        T Convert<T>(object value);

        object Convert(object value, Type targetType);
    }
}
