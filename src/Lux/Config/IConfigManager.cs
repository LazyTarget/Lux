namespace Lux.Config
{
    public interface IConfigManager
    {
        bool CanLoad<TConfig>(ConfigSource source)
            where TConfig : IConfig;
        TConfig Load<TConfig>(ConfigSource source)
            where TConfig : IConfig;

        bool CanSave<TConfig>(TConfig config, ConfigSource target)
            where TConfig : IConfig;
        object Save<TConfig>(TConfig config, ConfigSource target)
            where TConfig : IConfig;
    }
}