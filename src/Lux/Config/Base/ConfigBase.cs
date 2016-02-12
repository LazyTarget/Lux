namespace Lux.Config
{
    public abstract class ConfigBase : IConfig
    {
        //public IConfigArguments Arguments { get; set; }
        //public IConfigLocation Location { get; set; }
        public IConfigDescriptor Descriptor { get; set; }
    }
}