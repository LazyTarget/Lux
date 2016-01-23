using System;

namespace Lux.Interfaces
{
    public interface ITypeInstantiator
    {
        T Instantiate<T>();
        T Instantiate<T>(object[] arguments);
        T InstantiateWithParams<T>(params object[] arguments);

        object Instantiate(Type type);
        object Instantiate(Type type, object[] arguments);
        object InstantiateWithParams(Type type, params object[] arguments);
    }
}