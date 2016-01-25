using System;
using Lux.Interfaces;

namespace Lux
{
    public class TypeInstantiator : ITypeInstantiator
    {
        public bool ThrowOnError { get; set; }


        public T Instantiate<T>()
        {
            var res = Instantiate<T>(null);
            return res;
        }

        public virtual T Instantiate<T>(object[] arguments)
        {
            var obj = Instantiate(typeof(T), arguments);
            var res = (T)obj;
            return res;
        }

        public T InstantiateWithParams<T>(params object[] arguments)
        {
            var obj = Instantiate<T>(arguments);
            var res = (T)obj;
            return res;
        }



        public object Instantiate(Type type)
        {
            var res = Instantiate(type, null);
            return res;
        }

        public virtual object Instantiate(Type type, object[] arguments)
        {
            try
            {
                object obj;
                if (arguments != null)
                    obj = Activator.CreateInstance(type, arguments);
                else
                    obj = Activator.CreateInstance(type);
                return obj;
            }
            catch (Exception ex)
            {
                if (ThrowOnError)
                    throw;
                return null;
            }
        }

        public object InstantiateWithParams(Type type, params object[] arguments)
        {
            var obj = Instantiate(type, arguments);
            return obj;
        }
    }
}
