using System.Collections.Generic;
using Lux.Data;

namespace Lux.Config
{
    public interface IConfigDescriptor
    {
        IDictionary<string, object> Properties { get; set; }
        IDataStore<IConfigDescriptor> DataStore { get; set; }
        IConfigParser Parser { get; set; }
    }
}