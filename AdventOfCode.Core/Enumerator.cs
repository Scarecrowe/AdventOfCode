namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Contracts;

    public static class Enumerator
    {
        public static IEnumerable<(int i, int j)> Range2D(int length)
        {
            length.Should().Not().BeLessThanZero(paramName: nameof(length));

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    yield return new(i, j);
                }
            }
        }

        public static IEnumerable<(int x, int y)> Range2D(int width, int height)
        {
            width.Should().Not().BeLessThanZero(paramName: nameof(width));
            height.Should().Not().BeLessThanZero(paramName: nameof(height));

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    yield return new(x, y);
                }
            }
        }

        public static IEnumerable<(int i, int j, int k)> Range3D(int length)
        {
            length.Should().Not().BeLessThanZero(paramName: nameof(length));

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        yield return new(i, j, k);
                    }
                }
            }
        }
    }
}
