using System;

namespace Lux.Diagnostics.Log4net
{
    public class Log4NetLogFactory : ILogFactory
    {
        private readonly ILog4NetLogFactoryInitializer _initializer;

        public Log4NetLogFactory()
            : this(new Log4NetXmlConfiguratorInitializer())
        {
            
        }

        public Log4NetLogFactory(ILog4NetLogFactoryInitializer initializer)
        {
            //if (initializer == null)
            //    throw new ArgumentNullException(nameof(initializer));
            _initializer = initializer;
        }


        public void Init()
        {
            _initializer?.Initialize();
        }

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
