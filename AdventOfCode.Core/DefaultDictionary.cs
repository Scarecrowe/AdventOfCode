namespace AdventOfCode.Core
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
        where TValue : new()
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (!this.TryGetValue(key, out var value) || value is null)
                {
                    value = new TValue();
                    base[key] = value;
                }

                return value;
            }

            set => base[key] = value;
        }
    }
}
