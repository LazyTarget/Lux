using System;

namespace Lux.Diagnostics
{
    public interface ILogFactory
    {
        void Init();
        ILog GetLog(string name);
        ILog GetLog(Type type);
    }
}
