using System;

namespace Lux.Config
{
    public interface IConfigDescriptorFactory
    {
        IConfigDescriptor CreateDescriptor(Type configType);

        IConfigDescriptor CreateDescriptor<TConfig>()
               where TConfig : IConfig;
    }
}
