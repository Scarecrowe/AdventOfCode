namespace AdventOfCode.Puzzles._2016.Day_24___Air_Duct_Spelunking
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class AirDuctSpelunking
    {
        public AirDuctSpelunking(string[] input)
        {
            this.InterfaceCount--;
            this.Map = new(input, (c) =>
            {
                if (char.IsNumber(c) && c > this.InterfaceCount)
                {
                    this.InterfaceCount++;
                }

                return c;
            });
        }

        public VectorDictionary<int, char> Map { get; }

        public int InterfaceCount { get; private set; }

        public long ShortestPath(bool reset = false)
        {
            long result = long.MaxValue;

            List<List<int>> permutations = this.GeneratePermutations();
            Dictionary<int, Dictionary<int, long>> paths = this.GeneratePaths();

            for (int j = 0; j < permutations.Count; j++)
            {
                List<int> permutation = permutations[j];

                long current = paths[0][permutation[0]];

                for (int i = 0; i < permutation.Count - 1; i++)
                {
                    current += paths[permutation[i]][permutation[i + 1]];
                }

                if (current < result && reset)
                {
                    current += paths[permutation.Last()][0];
                }

                result = Math.Min(result, current);
            }

            return result;
        }

        private Dictionary<int, Dictionary<int, long>> GeneratePaths()
        {
            Dictionary<int, Dictionary<int, long>> distances = new();

            foreach(Vector<int> point in Vector<int>.AxisEnumerator(this.InterfaceCount + 1, this.InterfaceCount + 1))
            {
                Vector<int> source = this.Map.FirstOrDefault(x => x.Value == point.Y.ToString()[0]).Key;
                Vector<int> destination = this.Map.FirstOrDefault(x => x.Value == point.X.ToString()[0]).Key;

                if (!distances.ContainsKey(point.Y))
                {
                    distances.Add(point.Y, new());
                }

                distances[point.Y].Add(point.X, this.FindPath(source, destination));
            }

            return distances;
        }

        private long FindPath(Vector<int> source, Vector<int> destination)
        {
            long result = long.MaxValue;
            bool[,] visited = new bool[this.Map.Width, this.Map.Height];
            Queue<(Vector<int> point, int count)> queue = new();

            queue.Enqueue((source, 0));

            while (queue.Count > 0)
            {
                (Vector<int> point, int count) = queue.Dequeue();
                count++;

                if (count > result)
                {
                    continue;
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(point))
                {
                    if (visited[adjacent.Point.X, adjacent.Point.Y] || adjacent.Value == '#')
                    {
                        continue;
                    }

                    if (adjacent.Point == destination)
                    {
                        result = Math.Min(result, count);
                        break;
                    }

                    visited[adjacent.Point.X, adjacent.Point.Y] = true;

                    queue.Enqueue((adjacent.Point, count));
                }
            }

            return result;
        }

        private List<List<int>> GeneratePermutations()
        {
            List<int> interfaces = new();

            for (int i = 1; i <= this.InterfaceCount; i++)
            {
                interfaces.Add(i);
            }

            return interfaces.Permutations(this.InterfaceCount);
        }
    }
}
