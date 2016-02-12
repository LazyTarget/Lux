namespace Lux.Config
{
    public interface IConfig
    {
        IConfigDescriptor Descriptor { get; set; }
        //IConfigArguments Arguments { get; set; }
        //IConfigLocation Location { get; set; }
    }
}