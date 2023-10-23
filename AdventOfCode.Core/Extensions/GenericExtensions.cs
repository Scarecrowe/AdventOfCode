namespace AdventOfCode.Core.Extensions
{
    public static class GenericExtensions
    {
        public static int ToInt<T>(this T value) => Convert.ToInt32(value);

        public static long ToLong<T>(this T value) => Convert.ToInt64(value);

        public static double ToDouble<T>(this T value) => Convert.ToDouble(value);

        public static T[][] EmptyArray<T>(this T[][] array, int width, int height)
        {
            T[][] result = new T[width][];

            for (int i = 0; i < width; i++)
            {
                result[i] = new T[height];
            }

            return result;
        }
    }
}
