using System;

namespace Lux.Diagnostics
{
    public class TraceLogFactory : ILogFactory
    {
        public ILog GetLog(string name)
        {
            var log = new TraceLog();
            return log;
        }

        public ILog GetLog(Type type)
        {
            var log = new TraceLog();
            return log;
        }
    }
}
