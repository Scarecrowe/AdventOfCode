namespace AdventOfCode.Core.Extensions
{
    public static class LongExtensions
    {
        public static long Factorial(this long value) => value == 0 ? 1 : value * Factorial(value - 1);

        public static IEnumerable<long> Factors(this long value)
        {
            for (int i = 1; i <= value; i++)
            {
                if (value % i == 0)
                {
                    yield return i;
                }
            }
        }

        public static long IncrementWrap(this long value, int length) => (value + (length - 1)) % length;

        public static string ToBinary(this long value) => Convert.ToString(value, 2);

        public static string ToString(this long value, int toBase) => Convert.ToString(value, toBase);

        public static long Abs(this long value) => Math.Abs(value);
    }
}
