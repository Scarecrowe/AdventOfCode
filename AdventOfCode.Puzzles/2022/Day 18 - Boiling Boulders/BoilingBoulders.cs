namespace AdventOfCode.Puzzles._2022.Day_18___Boiling_Boulders
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class BoilingBoulders
    {
        public BoilingBoulders(string[] input)
        {
            this.DefaultLava = ParseCubes(input);
            this.Lava = new(this.DefaultLava);
        }

        public static List<Vector<int>> Cardinals { get; private set; } = new List<Vector<int>> { new(1, 0, 0), new(-1, 0, 0), new(0, 1, 0), new(0, -1, 0), new(0, 0, 1), new(0, 0, -1) };

        public Lava Lava { get; private set; }

        public List<Vector<int>> DefaultLava { get; private set; }

        public int SurfaceArea()
        {
            int result = 0;
            foreach (var cube in this.DefaultLava)
            {
                int count = 0;

                foreach (var (x, y, z) in GetAdjacent(new(new Vector<int>(cube.X, cube.Y, cube.Z))))
                {
                    if (this.DefaultLava.Contains(new(x, y, z)))
                    {
                        count++;
                    }
                }

                result += 6 - count;
            }

            return result;
        }

        public int ExteriorSurfaceArea()
        {
            this.Lava = new(this.DefaultLava);
            this.Lava.SortAirPockets();

            return this.Lava.Aggregate(0, (total, cube) =>
            {
                if (cube.Value.Type == CubeType.None)
                {
                    return total;
                }

                foreach (Vector<int> cardinal in Cardinals)
                {
                    Cube current = new(cube.Key + cardinal);

                    if (!this.Lava.ContainsKey(current.Point) || this.Lava[current.Point].Type == CubeType.None)
                    {
                        total++;
                    }
                }

                return total;
            });
        }

        private static List<Vector<int>> ParseCubes(string[] input) => input.Select(x => new Vector<int>(x.Split(",").ToInt())).ToList();

        private static IEnumerable<(int X, int Y, int Z)> GetAdjacent(Cube cube)
        {
            yield return (cube.Point.X - 1, cube.Point.Y, cube.Point.Z);
            yield return (cube.Point.X + 1, cube.Point.Y, cube.Point.Z);
            yield return (cube.Point.X, cube.Point.Y - 1, cube.Point.Z);
            yield return (cube.Point.X, cube.Point.Y + 1, cube.Point.Z);
            yield return (cube.Point.X, cube.Point.Y, cube.Point.Z - 1);
            yield return (cube.Point.X, cube.Point.Y, cube.Point.Z + 1);
        }
    }
}
