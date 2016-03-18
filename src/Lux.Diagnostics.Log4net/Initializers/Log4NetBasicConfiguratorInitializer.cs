namespace Lux.Diagnostics.Log4net
{
    public class Log4NetBasicConfiguratorInitializer : ILog4NetLogFactoryInitializer
    {
        public void Initialize()
        {
            var c = log4net.Config.BasicConfigurator.Configure();
        }
    }
}
