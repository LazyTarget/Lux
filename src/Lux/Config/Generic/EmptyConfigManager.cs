using Lux.Data;

namespace Lux.Config
{
    public class EmptyConfigManager : IConfigManager
    {
        public virtual bool CanLoad<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig
        {
            return false;
        }

        public virtual TConfig Load<TConfig>(IConfigDescriptor descriptor)
            where TConfig : IConfig
        {
            var res = default(TConfig);
            return res;
        }

        public virtual bool CanSave<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig
        {
            return false;
        }

        public virtual object Save<TConfig>(TConfig config, IDataStore<IConfigDescriptor> dataStore)
            where TConfig : IConfig
        {
            return null;
        }
    }
}
