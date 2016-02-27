using System;

namespace Lux.Dependency
{
    public interface IDependencyContainer : IDisposable
    {
        IDependencyContainer Parent { get; }
        IDependencyContainer CreateChildContainer();

        IDependencyContainer RegisterInstance(Type type, string name, object instance, object settings);
        IDependencyContainer RegisterType(Type from, Type to, string name, object settings);
        object GetRegistrations();
        object Resolve(Type type, string name, object settings);
    }
}
