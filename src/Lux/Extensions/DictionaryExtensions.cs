using System.Collections.Generic;
using System.Linq;

namespace Lux.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var res = GetOrDefault(dictionary, key, default(TValue));
            return res;
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            var res = defaultValue;
            if (dictionary.ContainsKey(key))
            {
                res = dictionary[key];
            }
            return res;
        }

    }
}
