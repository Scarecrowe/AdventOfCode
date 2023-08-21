namespace AdventOfCode.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static int[] ToInt(this IEnumerable<string> value) => value.Select(x => int.Parse($"{x}")).ToArray();

        public static uint[] ToUInt(this IEnumerable<string> value) => value.Select(x => uint.Parse($"{x}")).ToArray();

        public static long[] ToLong(this IEnumerable<string> value) => value.Select(x => long.Parse($"{x}")).ToArray();

        public static ulong[] ToULong(this IEnumerable<string> value) => value.Select(x => ulong.Parse($"{x}")).ToArray();

        public static float[] ToFloat(this IEnumerable<string> value) => value.Select(x => float.Parse($"{x}")).ToArray();

        public static double[] ToDouble(this IEnumerable<string> value) => value.Select(x => double.Parse($"{x}")).ToArray();

        public static decimal[] ToDecimal(this IEnumerable<string> value) => value.Select(x => decimal.Parse($"{x}")).ToArray();

        public static byte[] ToByte(this IEnumerable<string> value) => value.Select(x => byte.Parse($"{x}")).ToArray();

        public static List<int> ToIntList(this IEnumerable<string> value) => value.Select(x => int.Parse($"{x}")).ToList();

        public static List<long> ToLongList(this IEnumerable<string> value) => value.Select(x => long.Parse($"{x}")).ToList();

        public static List<float> ToFloatList(this IEnumerable<string> value) => value.Select(x => float.Parse($"{x}")).ToList();

        public static List<double> ToDoubleList(this IEnumerable<string> value) => value.Select(x => double.Parse($"{x}")).ToList();

        public static List<decimal> ToDecimalList(this IEnumerable<string> value) => value.Select(x => decimal.Parse($"{x}")).ToList();

        public static List<byte> ToByteList(this IEnumerable<string> value) => value.Select(x => byte.Parse($"{x}")).ToList();

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> collection, T splitOn)
        {
            List<T> current = new();

            foreach (T? item in collection)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.Equals(splitOn))
                {
                    yield return current;
                    current = new();
                }
                else
                {
                    current.Add(item);
                }
            }

            yield return current;
        }

        public static IEnumerable<byte[]> HashSequence(this IEnumerable<byte> lengths, int repeat)
        {
            var size = 256;
            var position = 0;
            var skip = 0;
            var state = Enumerable.Range(0, size).Select(i => (byte)i).ToArray();

            yield return state;

            for (int i = 0; i < repeat; i++)
            {
                foreach (var length in lengths)
                {
                    if (length > 1)
                    {
                        state = state.Select((v, i) => ((i < position && i + size >= position + length) || i >= position + length) ? v : state[((2 * position) + length + size - i - 1) % size]).ToArray();
                    }

                    yield return state;

                    position = (position + length + skip++) % size;
                }
            }
        }

        public static IEnumerable<T> TakeCount<T>(this IEnumerable<T> enumerable, int count) => enumerable.Take(enumerable.Count() - count);

        public static string Join<T>(this IEnumerable<T> enumerable, string seperator = "") => string.Join(seperator, enumerable);

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T value in enumerable)
            {
                action(value);
            }
        }

        public static long Product<T>(this IEnumerable<T> enumerable)
        {
            if (!enumerable.Any())
            {
                return 0;
            }

            long product = Convert.ToInt64(enumerable.ElementAt(0));

            for (int i = 1; i < enumerable.Count(); i++)
            {
                if (typeof(T) == typeof(int))
                {
                    product *= Convert.ToInt64(enumerable.ElementAt(i));
                }
            }

            return product;
        }
    }
}