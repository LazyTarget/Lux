using System;

namespace Lux.Diagnostics
{
    public interface ILogFactory
    {
        ILog GetLog(string name);
        ILog GetLog(Type type);
    }
}
