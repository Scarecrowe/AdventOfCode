namespace AdventOfCode.Animation.Terraria.Random
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public abstract class RandomAsset
    {
        public RandomAsset(
            Tileset tileset,
            Frequency frequency)
        {
            this.Tileset = tileset;
            this.Frequency = frequency;
        }

        public RandomAsset(
            Tileset tileset,
            Frequency frequency,
            int startIndex,
            int endIndex)
        {
            this.Tileset = tileset;
            this.Frequency = frequency;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }

        public Tileset Tileset { get; }

        public Frequency Frequency { get; }

        public double FrequencyEnd { get; }

        public int? StartIndex { get; protected set; }

        public int? EndIndex { get; protected set; }

        protected static bool IsValid(VectorArray<long, EntityType> map, Vector<long> point)
            => map[new(point.X + 1, point.Y - 1)] != EntityType.Clay;

        protected int GetRandomIndex()
        {
            if (!this.StartIndex.HasValue
                || !this.EndIndex.HasValue)
            {
                return RandomGenerator.Next(0, this.Tileset.Count - 1);
            }

            return RandomGenerator.Next(this.StartIndex.Value, this.EndIndex.Value);
        }

        protected Vector<long> RenderPoint(Vector<long> point)
        {
            long x = point.X * 16;
            long y = ((point.Y - 1) * 16) - (this.Tileset.TileHeight - 16);

            return new(x, y);
        }
    }
}
