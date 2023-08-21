namespace AdventOfCode.Core.Extensions
{
    public static class GenericExtensions
    {
        public static int ToInt<T>(this T value) => Convert.ToInt32(value);

        public static long ToLong<T>(this T value) => Convert.ToInt64(value);
    }
}
