using System;
using System.Collections.Generic;

namespace Lux
{
    public class DictionaryPairChangedEventArgs<TKey, TValue> : EventArgs
    {
        public DictionaryPairChangedEventArgs(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Dictionary = dictionary;
            Key = key;
            Value = value;

            Removed = !Dictionary.ContainsKey(key);
        }

        public IDictionary<TKey, TValue> Dictionary { get; private set; }
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }
        public bool Removed { get; private set; }
    }
}
