namespace AdventOfCode.Core.Extensions
{
    public static class IntExtensions
    {
        public static int Factorial(this int value) => value == 0 ? 1 : value * Factorial(value - 1);

        public static IEnumerable<int> Factors(this int value)
        {
            for (int i = 1; i <= value; i++)
            {
                if (value % i == 0)
                {
                    yield return i;
                }
            }
        }

        public static int IncrementWrap(this int value, int length) => (value + (length - 1)) % length;

        public static T ToGeneric<T>(this int value)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }

            return (T)(object)Convert.ToInt64(value);
        }

        public static T ToGeneric<T>(this long value)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }

            return (T)(object)Convert.ToInt64(value);
        }

        public static string ToBinary(this int value) => Convert.ToString(value, 2);

        public static string ToString(this int value, int toBase) => Convert.ToString(value, toBase);

        public static int Abs(this int value) => Math.Abs(value);

        public static int Toggle(this int value) => value == 1 ? 0 : 1;

        public static int ZeroIfNegative(this int value) => value >= 0 ? value : 0;
    }
}
