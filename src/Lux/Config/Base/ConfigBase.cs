namespace Lux.Config
{
    public abstract class ConfigBase : IConfig
    {
        public IConfigDescriptor Descriptor { get; set; }
    }
}