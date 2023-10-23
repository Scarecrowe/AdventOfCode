namespace AdventOfCode.Animation
{
    public static class RandomGenerator
    {
        public static int Seed { get; set; }

        private static Random? Random { get; set; }

        public static void Initialise(int seed)
        {
            Seed = seed;
            Random = new Random(seed);
        }

        public static void Initialise() => Initialise(DateTime.Now.Ticks.GetHashCode());

        public static int Next(int minValue, int maxValue)
            => Random?.Next(minValue, maxValue) ?? 0;

        public static bool TrueOrFalse() => Random?.NextDouble() >= 0.5;

        public static double NextDouble() => Random?.NextDouble() ?? 0.0;
    }
}
