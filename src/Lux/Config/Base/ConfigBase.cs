namespace Lux.Config
{
    public abstract class ConfigBase : IConfig
    {
        public IConfigLocation Location { get; set; }
    }
}