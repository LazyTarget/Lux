using System;

namespace Lux.Diagnostics
{
    public class DebugLogFactory : ILogFactory
    {
        public void Init()
        {
            
        }

        public ILog GetLog(string name)
        {
            var log = new DebugLog();
            return log;
        }

        public ILog GetLog(Type type)
        {
            var log = new DebugLog();
            return log;
        }
    }
}
