using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassowary.Utils
{
    internal static class DictionaryEx
    {
        internal static TValue GetOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return default(TValue);
        }
    }
}
