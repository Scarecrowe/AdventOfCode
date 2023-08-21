namespace AdventOfCode.Core.Extensions
{
    public static class HashSetExtensions
    {
        public static void AddRange<TValue>(this HashSet<TValue> hashSet, HashSet<TValue> values)
        {
            foreach (TValue value in values)
            {
                hashSet.Add(value);
            }
        }
    }
}
