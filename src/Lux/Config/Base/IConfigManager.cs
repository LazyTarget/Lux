namespace Lux.Config
{
    public interface IConfigManager
    {
        bool CanLoad<TConfig>(IConfigLocation location)
            where TConfig : IConfig;
        TConfig Load<TConfig>(IConfigLocation location)
            where TConfig : IConfig;

        bool CanSave<TConfig>(TConfig config, IConfigLocation location)
            where TConfig : IConfig;
        object Save<TConfig>(TConfig config, IConfigLocation location)
            where TConfig : IConfig;
    }
}