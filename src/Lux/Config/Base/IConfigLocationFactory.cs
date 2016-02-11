namespace Lux.Config
{
    public interface IConfigLocationFactory
    {
        IConfigLocation CreateLocation<TConfig>()
            where TConfig : IConfig;
    }
}
