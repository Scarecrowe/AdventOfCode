namespace AdventOfCode.Puzzles._2025.Day_08___Playground
{
    using AdventOfCode.Core;

    public class Playground
    {
        public List<Vector<long>> Junctions { get; private set; } = [];

        public List<(int indexA, int indexB)> Edges { get; private set; } = [];

        public Playground(string[] input)
        {
            this.Junctions = Parse(input);
            this.Edges = this.GetEdges();
        }

        public long MultiplyTopThreeCircuit()
        {
            Dictionary<int, int> sets = [];
            DisjointSetUnion set = new(this.Junctions.Count);
            int connections = 0;

            foreach (var (indexA, indexB) in this.Edges)
            {
                if (connections == this.Junctions.Count)
                {
                    for (int i = 0; i < this.Junctions.Count; i++)
                    {
                        int root = set.Find(i);

                        if (!sets.TryGetValue(root, out int value))
                        {
                            value = 0;
                            sets[root] = value;
                        }

                        sets[root] = ++value;
                    }

                    List<int> sizes = [.. sets.Values.OrderByDescending(x => x)];

                    while (sizes.Count < 3)
                    {
                        sizes.Add(1);
                    }

                    return (long)sizes[0] * sizes[1] * sizes[2];
                }

                set.Union(indexA, indexB);
                connections++;
            }

            sets.Clear();

            for (int i = 0; i < this.Junctions.Count; i++)
            {
                int root = set.Find(i);

                if (!sets.TryGetValue(root, out int value))
                {
                    value = 0;
                    sets[root] = value;
                }

                sets[root] = ++value;
            }

            List<int> result =
                sets.Values
                    .OrderByDescending(x => x)
                    .Take(3)
                    .Concat(Enumerable.Repeat(1, 3))
                    .Take(3)
                    .ToList();

            return (long)result[0] * result[1] * result[2];
        }

        public long MultiplyXCoordinates()
        {
            DisjointSetUnion set = new(this.Junctions.Count);

            foreach (var (indexA, indexB) in this.Edges)
            {
                if (set.Union(indexA, indexB) && set.Sets == 1)
                {
                    return (long)this.Junctions[indexA].X * this.Junctions[indexB].X;
                }
            }

            return 0;
        }

        private static List<Vector<long>> Parse(string[] input)
            => input.Select(x => new Vector<long>(x.Split(',').Select(int.Parse).ToArray())).ToList();

        private List<(int indexA, int indexB)> GetEdges()
        {
            List<(int indexA, int indexB)> result = [];

            for (int i = 0; i < this.Junctions.Count; i++)
            {
                Vector<long> a = this.Junctions[i];

                for (int j = i + 1; j < this.Junctions.Count; j++)
                {
                    result.Add((i, j));
                }
            }

            result.Sort((a, b) =>
            {
                Vector<long> pointA = this.Junctions[a.indexA];
                Vector<long> pointB = this.Junctions[a.indexB];
                Vector<long> pointC = this.Junctions[b.indexA];
                Vector<long> pointD = this.Junctions[b.indexB];

                long dx1 = pointA.X - pointB.X;
                long dy1 = pointA.Y - pointB.Y;
                long dz1 = pointA.Z - pointB.Z;

                long dx2 = pointC.X - pointD.X;
                long dy2 = pointC.Y - pointD.Y;
                long dz2 = pointC.Z - pointD.Z;

                long distanceA = dx1 * dx1 + dy1 * dy1 + dz1 * dz1;
                long distanceB = dx2 * dx2 + dy2 * dy2 + dz2 * dz2;

                return distanceA.CompareTo(distanceB);
            });

            return result;
        }
    }
}
