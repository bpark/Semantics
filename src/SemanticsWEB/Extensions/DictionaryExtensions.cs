using System;
using System.Collections.Generic;

namespace SemanticsWEB.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue ComputeIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> mappingFunction)
        {
            if (dictionary.TryGetValue(key, out var result)) return result;
            result = mappingFunction(key);
            dictionary.Add(key, result);

            return result;
        }
    }
}