namespace Lux.Config
{
    public interface IConfig
    {
        IConfigArguments Arguments { get; set; }
        IConfigLocation Location { get; set; }
    }
}