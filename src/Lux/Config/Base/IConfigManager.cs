using Lux.Data;

namespace Lux.Config
{
    public interface IConfigManager
    {
        //IConfigDescriptor GetDefaultDescriptor<TConfig>()
        //    where TConfig : IConfig;

        bool CanLoad<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig;
        TConfig Load<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig;

        bool CanSave<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig;
        object Save<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig;
    }
}