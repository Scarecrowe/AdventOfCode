namespace AdventOfCode.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static void TryAddValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
            where TKey : notnull
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> value)
            where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> pair in value)
            {
                dict.TryAddValue(pair.Key, pair.Value);
            }
        }

        public static int GetKeyIndex<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey value)
            where TKey : notnull
            => dict.Keys.ToList().IndexOf(value);

        public static int GetValueIndex<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue value)
            where TKey : notnull
            => dict.Values.ToList().IndexOf(value);

        public static KeyValuePair<TKey, TValue> Pop<TKey, TValue>(this Dictionary<TKey, TValue> dict)
            where TKey : notnull
        {
            var last = dict.Last();

            dict.Remove(last.Key);

            return last;
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<KeyValuePair<TKey, TValue>> action)
            where TKey : notnull
        {
            foreach (KeyValuePair<TKey, TValue> pair in dict)
            {
                action(pair);
            }
        }
    }
}
