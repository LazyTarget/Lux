using System;

namespace Lux.Diagnostics.Log4net
{
    public class Log4NetLogFactory : ILogFactory
    {
        public ILog GetLog(string name)
        {
            var l = log4net.LogManager.GetLogger(name);
            var log = new Log4NetLog(l);
            return log;
        }

        public ILog GetLog(Type type)
        {
            var l = log4net.LogManager.GetLogger(type);
            var log = new Log4NetLog(l);
            return log;
        }
    }
}
