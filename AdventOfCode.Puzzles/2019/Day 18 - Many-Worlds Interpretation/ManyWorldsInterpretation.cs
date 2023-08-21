namespace AdventOfCode.Puzzles._2019.Day_18___Many_Worlds_Interpretation
{
    using System.Text;
    using AdventOfCode.Core;

    public class ManyWorldsInterpretation
    {
        public ManyWorldsInterpretation(string[] input)
        {
            this.Map = new(input, (c) => c);

            VectorCell<int, char>? cell = this.Map.FirstOrDefault('@');

            this.Locations = new() { new(cell?.Point ?? new(0, 0)) };
        }

        public List<Vector<int>> Locations { get; }

        public HashSet<char>? Inventory { get; }

        public VectorArray<int, char> Map { get; }

        public ManyWorldsInterpretation SplitMap()
        {
            Vector<int> location = this.Locations[0];

            this.Map[this.Locations[0].Y, this.Locations[0].X - 1] = '#';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X] = '#';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X] = '#';
            this.Map[this.Locations[0].Y, this.Locations[0].X + 1] = '#';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X - 1] = '@';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X - 1] = '@';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X + 1] = '@';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X + 1] = '@';

            this.Map[this.Locations[0].Y, this.Locations[0].X] = '#';
            this.Locations.Clear();

            this.Locations.Add(location - 1);
            this.Locations.Add(location + new Vector<int>(-1, 1));
            this.Locations.Add(location + new Vector<int>(1, -1));
            this.Locations.Add(location + new Vector<int>(1, 1));

            return this;
        }

        public long CollectKeys(Vector<int>? location = null, List<bool>? visitedState = null)
        {
            location ??= this.Locations[0];

            Queue<QueueItem> queue = new();
            HashSet<(Vector<int> Point, string Visited)> cache = new();

            queue.Enqueue(new(location, 0, visitedState?.ToArray() ?? new bool[26]));

            while (queue.Count > 0)
            {
                QueueItem current = queue.Dequeue();

                var key = (new Vector<int>(current.Location), VisitedKey(current.Visited));

                if (cache.Contains(key))
                {
                    continue;
                }

                cache.Add(key);

                if (AllVisited(current.Visited))
                {
                    return current.Distance;
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(current.Location).Where(c => c.Value != '#'))
                {
                    bool[] visited = (bool[])current.Visited.Clone();

                    if (IsDoor(adjacent.Value) && !current.Visited[adjacent.Value - 'A'])
                    {
                        continue;
                    }

                    if (IsKey(adjacent.Value))
                    {
                        visited[adjacent.Value - 'a'] = true;
                    }

                    queue.Enqueue(new(new(adjacent.Point), current.Distance + 1, visited));
                }
            }

            return -1;
        }

        public long CollectVaultKeys()
        {
            List<bool[]> visitedStart = new()
            {
                new bool[26],
                new bool[26],
                new bool[26],
                new bool[26]
            };

            this.IgnoreDoors(visitedStart);

            long result = 0;

            for (int i = 0; i < 4; i++)
            {
                result += this.CollectKeys(this.Locations[i], visitedStart[i].ToList());
            }

            return result;
        }

        private static bool IsDoor(char value) => value >= 65 && value <= 90;

        private static bool IsKey(char value) => value >= 97 && value <= 122;

        private static bool AllVisited(bool[] visited)
        {
            for (int i = 0; i < 26; i++)
            {
                if (!visited[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string VisitedKey(bool[] visited)
        {
            StringBuilder sb = new();

            for (int i = 0; i < 26; i++)
            {
                sb.Append(visited[i] ? 1 : 0);
            }

            return sb.ToString();
        }

        private void IgnoreDoors(List<bool[]> visitedStart)
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (IsKey(this.Map[y, x]))
                    {
                        visitedStart[0][this.Map[y, x] - 'a'] = !((x <= this.Locations[0].X) && (y <= this.Locations[0].Y));
                        visitedStart[1][this.Map[y, x] - 'a'] = !((x <= this.Locations[1].X) && (y >= this.Locations[1].Y));
                        visitedStart[2][this.Map[y, x] - 'a'] = !((x >= this.Locations[2].X) && (y <= this.Locations[2].Y));
                        visitedStart[3][this.Map[y, x] - 'a'] = !((x >= this.Locations[3].X) && (y >= this.Locations[3].Y));
                    }
                }
            }
        }
    }
}
