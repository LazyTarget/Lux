using System;

namespace Lux.Diagnostics
{
    public class NullObjectLogFactory : ILogFactory
    {
        public void Init()
        {
            
        }

        public ILog GetLog(string name)
        {
            var log = new NullObjectLog();
            return log;
        }

        public ILog GetLog(Type type)
        {
            var log = new NullObjectLog();
            return log;
        }
    }
}
