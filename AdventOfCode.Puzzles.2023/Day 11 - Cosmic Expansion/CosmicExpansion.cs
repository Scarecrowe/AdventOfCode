namespace AdventOfCode.Puzzles._2023.Day_11___Cosmic_Expansion
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class CosmicExpansion
    {
        public CosmicExpansion(string[] input)
        {
            this.Galaxies = new VectorDictionary<int, char>(input, (c) => c)
                .AxisEnumerator()
                .Where(x => x.Value == '#')
                .Select(x => x.Point)
                .ToList();
            this.Rows = Enumerable.Range(0, input.Length)
                .Where(x => !this.Galaxies.Any(y => y.Y == x))
                .ToList();
            this.Cols = Enumerable.Range(0, input[0].Length)
                .Where(x => !this.Galaxies.Any(y => y.X == x))
                .ToList();
        }

        public List<Vector<int>> Galaxies { get; }

        public List<int> Cols { get; }

        public List<int> Rows { get; }

        public long SumOfShortestPath(long amount)
        {
            long result = 0;
            HashSet<(Vector<int>, Vector<int>)> visited = new();

            foreach(var (pointA, pointB) in this.Galaxies.PairEnumerator())
            {
                if (!visited.Contains((pointA, pointB))
                      && !visited.Contains((pointB, pointA)))
                {
                    result += pointA.Distance(pointB)
                        + (((int)amount - 1) * this.Cols.Count(c => c > Math.Min(pointA.X, pointB.X) && c < Math.Max(pointA.X, pointB.X)))
                        + (((int)amount - 1) * this.Rows.Count(c => c > Math.Min(pointA.Y, pointB.Y) && c < Math.Max(pointA.Y, pointB.Y)));

                    visited.Add((pointA, pointB));
                }
            }

            return result;
        }
    }
}
