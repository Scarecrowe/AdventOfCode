namespace AdventOfCode.Puzzles._2021.Day_09___Smoke_Basin
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SmokeBasin
    {
        public SmokeBasin(string[] input) => this.Map = ParseInput(input);

        private VectorArray<int, int> Map { get; }

        public int SumOfRiskLevels() => this.Lowest().Sum(point => this.Map[point.Y, point.X] + 1);

        public long SumOfBasin()
        {
            long total = 0;

            foreach (HashSet<Vector<int>> basin in this.Basins().OrderByDescending(x => x.Count).Take(3))
            {
                total = total == 0 ? basin.Count : total * basin.Count;
            }

            return total;
        }

        private static VectorArray<int, int> ParseInput(string[] input) => new(input, (c) => $"{c}".ToInt());

        private bool IsLowest(Vector<int> point)
        {
            int height = this.Map[point];

            foreach (VectorCell<int, int> adjacent in this.Map.AdjacentCardinal(point))
            {
                if (this.Map[adjacent.Point] <= height)
                {
                    return false;
                }
            }

            return true;
        }

        private HashSet<Vector<int>> Lowest()
        {
            HashSet<Vector<int>> lowest = new();

            for (int y = 0; y < this.Map.Height; ++y)
            {
                for (int x = 0; x < this.Map.Width; ++x)
                {
                    if (this.IsLowest(new(x, y)))
                    {
                        lowest.Add(new(x, y));
                    }
                }
            }

            return lowest;
        }

        private HashSet<Vector<int>> Basin(Vector<int> lowest, HashSet<Vector<int>> basin)
        {
            if (!basin.Contains(lowest) && this.Map[lowest] != 9)
            {
                basin.Add(lowest);

                foreach (VectorCell<int, int> adjacent in this.Map.AdjacentCardinal(lowest))
                {
                    foreach (Vector<int> basinAdjacent in this.Basin(adjacent.Point, basin))
                    {
                        basin.Add(basinAdjacent);
                    }
                }
            }

            return basin;
        }

        private HashSet<HashSet<Vector<int>>> Basins() => this.Lowest().Select(x => this.Basin(x, new())).ToHashSet();
    }
}
