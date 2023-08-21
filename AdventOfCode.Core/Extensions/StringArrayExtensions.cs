namespace AdventOfCode.Core.Extensions
{
    public static class StringArrayExtensions
    {
        public static int[] ToInt(this string[] value) => value.Select(x => int.Parse($"{x}")).ToArray();

        public static uint[] ToUInt(this string[] value) => value.Select(x => uint.Parse($"{x}")).ToArray();

        public static long[] ToLong(this string[] value) => value.Select(x => long.Parse($"{x}")).ToArray();

        public static ulong[] ToULong(this string[] value) => value.Select(x => ulong.Parse($"{x}")).ToArray();

        public static float[] ToFloat(this string[] value) => value.Select(x => float.Parse($"{x}")).ToArray();

        public static double[] ToDouble(this string[] value) => value.Select(x => double.Parse($"{x}")).ToArray();

        public static decimal[] ToDecimal(this string[] value) => value.Select(x => decimal.Parse($"{x}")).ToArray();

        public static byte[] ToByte(this string[] value) => value.Select(x => byte.Parse($"{x}")).ToArray();

        public static List<int> ToIntList(this string[] value) => value.Select(x => int.Parse($"{x}")).ToList();

        public static List<long> ToLongList(this string[] value) => value.Select(x => long.Parse($"{x}")).ToList();

        public static List<float> ToFloatList(this string[] value) => value.Select(x => float.Parse($"{x}")).ToList();

        public static List<double> ToDoubleList(this string[] value) => value.Select(x => double.Parse($"{x}")).ToList();

        public static List<decimal> ToDecimalList(this string[] value) => value.Select(x => decimal.Parse($"{x}")).ToList();

        public static List<byte> ToByteList(this string[] value) => value.Select(x => byte.Parse($"{x}")).ToList();

        public static void ForEach<T>(this T[] enumerable, Action<T> action)
        {
            foreach (T value in enumerable)
            {
                action(value);
            }
        }
    }
}
