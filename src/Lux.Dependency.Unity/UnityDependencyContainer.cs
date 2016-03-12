using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Lux.Dependency.Unity
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private UnityDependencyContainer _parent;
        private readonly IUnityContainer _container;

        public UnityDependencyContainer()
            : this(new UnityContainer())
        {
        }

        public UnityDependencyContainer(IUnityContainer unityContainer)
        {
            _container = unityContainer;
        }



        public IDependencyContainer Parent
        {
            get { return _parent; }
        }

        public IDependencyContainer CreateChildContainer()
        {
            var cont = _container.CreateChildContainer();
            var childContainer = new UnityDependencyContainer(cont);
            childContainer._parent = this;
            return childContainer;
        }

        public IDependencyContainer RegisterInstance(Type type, string name, object instance, object settings)
        {
            var lifetime = settings as LifetimeManager;
            if (lifetime == null)
            {
                var set = settings as UnityDependencyContainerSettings;
                if (set != null)
                    lifetime = set.LifetimeManager;
            }

            _container.RegisterInstance(type, name, instance, lifetime);
            return this;
        }

        public IDependencyContainer RegisterType(Type @from, Type to, string name, object settings)
        {
            var set = settings as UnityDependencyContainerSettings;

            var lifetime = settings as LifetimeManager;
            if (lifetime == null)
                lifetime = set?.LifetimeManager;

            var injectionMembers = settings as IEnumerable<InjectionMember>;
            if (injectionMembers == null)
                injectionMembers = set?.InjectionMembers;
            
            _container.RegisterType(@from, to, name, lifetime, injectionMembers?.ToArray());
            return this;
        }

        public object GetRegistrations()
        {
            var registrations = _container.Registrations;
            return registrations;
        }

        public object Resolve(Type type, string name, object settings)
        {
            var set = settings as UnityDependencyContainerSettings;

            var overrides = settings as IEnumerable<ResolverOverride>;
            if (overrides == null)
                overrides = set?.ResolverOverrides;

            var obj = _container.Resolve(type, overrides?.ToArray());
            return obj;
        }

        public void Dispose()
        {
            _container.Dispose();
        }


        public class UnityDependencyContainerSettings
        {
            public LifetimeManager LifetimeManager;
            public InjectionMember[] InjectionMembers;
            public ResolverOverride[] ResolverOverrides;
        }

    }
}
