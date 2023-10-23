namespace AdventOfCode.Core.Extensions
{
    public static class DoubleExtensions
    {
        public static double Factorial(this double value) => value == 0 ? 1 : value * Factorial(value - 1);

        public static IEnumerable<double> Factors(this double value)
        {
            for (int i = 1; i <= value; i++)
            {
                if (value % i == 0)
                {
                    yield return i;
                }
            }
        }

        public static double IncrementWrap(this double value, int length) => (value + (length - 1)) % length;

        public static double Abs(this double value) => Math.Abs(value);

        public static double ZeroIfNegative(this double value) => value >= 0L ? value : 0L;
    }
}
