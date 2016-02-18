using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Lux.Config;

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


        /// <summary>
        /// Merges two NameValueCollections.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <remarks>UseConfigSystemion.ConfigSystem">ConfigSystem</c> to merge AppSettings</remarks>
        public static NameValueCollection Merge(this NameValueCollection first, NameValueCollection second)
        {
            if (second == null)
                return first;
            if (ReferenceEquals(first, second))
                return first;

            foreach (string item in second)
            {
                if (first.AllKeys.Contains(item))
                {
                    // if first already contains this item, update it to the value of second
                    first[item] = second[item];
                }
                else
                {
                    // otherwise add it
                    first.Add(item, second[item]);
                }
            }
            return first;
        }


        /// <summary>
        /// Merges two ConnectionStringSettingsCollections.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static ConnectionStringSettingsCollection Merge(this ConnectionStringSettingsCollection first, ConnectionStringSettingsCollection second)
        {
            if (second == null)
                return first;
            if (ReferenceEquals(first, second))
                return first;

            foreach (ConnectionStringSettings item in second)
            {
                ConnectionStringSettings itemInSecond = item;
                ConnectionStringSettings existingItem = first.Cast<ConnectionStringSettings>().FirstOrDefault(x => x.Name == itemInSecond.Name);

                if (existingItem != null)
                {
                    first.Remove(item);
                }

                first.Add(item);
            }

            return first;
        }

    }
}
