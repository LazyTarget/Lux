using System;
using Lux;
using Lux.Extensions;

namespace Lux.Config
{
    public interface IConfigParser
    {
        TConfig Parse<TConfig>(IConfigDescriptor descriptor, object data)
            where TConfig : IConfig;

        object Export<TConfig>(TConfig config, object data)
            where TConfig : IConfig;
    }
}
