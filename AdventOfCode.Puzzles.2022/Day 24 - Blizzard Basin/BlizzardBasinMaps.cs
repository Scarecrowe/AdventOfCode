namespace AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin
{
    using AdventOfCode.Core;

    public class BlizzardBasinMaps
    {
        public BlizzardBasinMaps()
        {
            this.Point = new(0, 0);
            this.Maps = new();
        }

        public BlizzardBasinMaps(List<VectorArray<int, BlizzardBasinType>> maps)
        {
            this.Point = new Vector<int>((int)maps[0].Width, (int)maps[0].Height);
            this.Maps = maps;
        }

        public Vector<int> Point { get; }

        private List<VectorArray<int, BlizzardBasinType>> Maps { get; }

        public static VectorArray<int, BlizzardBasinType> Build(string[] input, List<Blizzard> blizzards)
        {
            var result = new VectorArray<int, BlizzardBasinType>(input, (c) =>
            {
                return c switch
                {
                    '<' or '>' or 'v' or '^' => BlizzardBasinType.Empty,
                    _ => (BlizzardBasinType)c,
                };
            });

            foreach (Blizzard blizzard in blizzards)
            {
                result[blizzard.Point.Y, blizzard.Point.X] = BlizzardBasinType.Blizzard;
            }

            return result;
        }

        public bool IsValid(Vector<int> point) =>
           point.Y >= 0
           && point.Y < this.Point.Y
           && point.X >= 0
           && point.X < this.Point.X;

        public BlizzardBasinType GetMap(int time, Vector<int> point)
            => this.IsValid(point) ? this.Maps[time % this.Maps.Count][(int)point.Y, (int)point.X] : BlizzardBasinType.Rock;
    }
}
