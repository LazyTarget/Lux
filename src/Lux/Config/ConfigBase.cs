namespace Lux.Config
{
    public abstract class ConfigBase : IConfig
    {
        public ConfigSource Source { get; set; }
    }
}