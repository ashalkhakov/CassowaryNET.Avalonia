using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoLayoutPanel
{
    internal static class DictionaryEx
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
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
