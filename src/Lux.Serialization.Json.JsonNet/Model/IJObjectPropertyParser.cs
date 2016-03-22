using System;
using System.Collections.Generic;
using Lux.Model;
using Newtonsoft.Json.Linq;

namespace Lux.Serialization.Json.JsonNet
{
    public interface IJObjectPropertyParser
    {
        IEnumerable<IProperty> Parse(JObject obj);
        IProperty DefineProperty(JObject obj, string propertyName, Type type, object value, bool isReadOnly);
        void ClearProperties(JObject obj);
    }
}