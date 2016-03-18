using System;
using System.IO;

namespace Lux.Diagnostics.Log4net
{
    public class Log4NetXmlConfiguratorInitializer : ILog4NetLogFactoryInitializer
    {
        public FileInfo FileInfo { get; set; }

        public bool Watch { get; set; }

        public void Initialize()
        {
            if (Watch)
            {
                if (FileInfo != null)
                    log4net.Config.XmlConfigurator.ConfigureAndWatch(FileInfo);
                else
                    throw new ArgumentException("FileInfo is required when Watch is enabled");
            }
            else
            {
                if (FileInfo != null)
                    log4net.Config.XmlConfigurator.Configure(FileInfo);
                else
                    log4net.Config.XmlConfigurator.Configure();
            }
        }
    }
}
