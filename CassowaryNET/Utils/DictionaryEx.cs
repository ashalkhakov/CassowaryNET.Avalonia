using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CassowaryNET.Utils
{
    internal static class DictionaryEx
    {
        [Pure]
        internal static TValue GetOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
            where TValue : class
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return defaultValue;
        }

        internal static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> valueFactory)
            where TValue : class
        {
            TValue value;

            if (dictionary.TryGetValue(key, out value)) 
                return value;

            value = valueFactory(key);
            dictionary.Add(key, value);
            return value;
        }
    }
}
