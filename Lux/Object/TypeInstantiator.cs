using System;
using Lux.Interfaces;

namespace Lux
{
    public class TypeInstantiator : ITypeInstantiator
    {
        public bool ThrowOnError { get; set; }


        public T Instantiate<T>()
        {
            var obj = Instantiate(typeof(T));
            var res = (T)obj;
            return res;
        }

        public T Instantiate<T>(params object[] arguments)
        {
            var obj = Instantiate(typeof(T), arguments);
            var res = (T)obj;
            return res;
        }


        public object Instantiate(Type type)
        {
            var obj = Instantiate(type, null);
            return obj;
        }

        public virtual object Instantiate(Type type, params object[] arguments)
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

    }
}
