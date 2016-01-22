using System;

namespace Lux.Interfaces
{
    public interface ITypeInstantiator
    {
        T Instantiate<T>();
        T Instantiate<T>(params object[] arguments);

        object Instantiate(Type type);
        object Instantiate(Type type, params object[] arguments);
    }
}