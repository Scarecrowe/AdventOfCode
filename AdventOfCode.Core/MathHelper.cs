namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Contracts;

    public class MathHelper
    {
        public static long GreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                long c = b;
                b = a % b;
                a = c;
            }

            return a;
        }

        public static int GreatestCommonFactor(int a, int b)
        {
            while (b != 0)
            {
                int c = b;
                b = a % b;
                a = c;
            }

            return a;
        }

        public static long LeastCommonMultiple(long a, long b) => (a / GreatestCommonFactor(a, b)) * b;

        public static int LeastCommonMultiple(int a, int b) => (a / GreatestCommonFactor(a, b)) * b;

        public static long LeastCommonMultiple(List<long> values)
        {
            values.Should().Not().BeNull(paramName: nameof(values));

            if (values.Count < 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            long value = values[0];

            for (int i = 1; i < values.Count; i++)
            {
                value = LeastCommonMultiple(value, values[i]);
            }

            return value;
        }

        public static int Min(params int[] values) => values.Min();

        public static long Min(params long[] values) => values.Min();

        public static int Max(params int[] values) => values.Max();

        public static long Max(params long[] values) => values.Max();

        public static int SurfaceArea(int width, int height, int length)
        {
            int a = length * width;
            int b = width * height;
            int c = length * height;

            return (a + b + c) * 2;
        }

        public static int SmallestSurface(int width, int height, int length)
        {
            int a = length * width;
            int b = width * height;
            int c = length * height;

            return Min(a, b, c);
        }

        public static int Volume(int width, int height, int length) => length * width * height;

        public static int SmallestPerimeter(int width, int height, int length)
        {
            int a = (length + width) * 2;
            int b = (width + height) * 2;
            int c = (length + height) * 2;

            return Min(a, b, c);
        }

        public static bool IsTriangle(int a, int b, int c) => a + b > c && b + c > a && a + c > b;
    }
}
