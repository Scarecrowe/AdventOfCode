namespace AdventOfCode.Puzzles._2019.Day_24___Planet_of_Discord
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class PlanetOfDiscord
    {
        public PlanetOfDiscord(string[] input)
        {
            this.Input = input;
            this.Map = new(input, (c) => c);
            this.Cache = new();
            this.Visited = new bool[0, 0];
        }

        private VectorArray<int, char> Map { get; set; }

        private HashSet<string> Cache { get; }

        private bool[,] Visited { get; set; }

        private string[] Input { get; }

        public long BiodiversityRating()
        {
            long result = 0;
            this.Cache.Add(this.Map.Flatten().Join());

            while (true)
            {
                VectorArray<int, char> map = new(this.Map.Width, this.Map.Height);

                foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
                {
                    if (cell.Value == '#')
                    {
                        map[cell.Point] = this.Map.AdjacentCardinal(cell.Point).Count(c => c.Value == '#') != 1 ? '.' : '#';
                    }
                    else
                    {
                        int count = this.Map.AdjacentCardinal(cell.Point).Count(c => c.Value == '#');
                        map[cell.Point] = (count >= 1 && count <= 2) ? '#' : '.';
                    }
                }

                string key = string.Join(string.Empty, map.Flatten());

                if (this.Cache.Contains(key))
                {
                    for (int i = 0; i < key.Length; i++)
                    {
                        if (key[i] == '#')
                        {
                            result += (long)Math.Pow(2, i);
                        }
                    }

                    return result;
                }
                else
                {
                    this.Cache.Add(key);
                }

                this.Map = map;
            }
        }

        public int BugCount()
        {
            this.Visited = new bool[this.Map.Height, this.Map.Width];
            for (var y = 0; y < this.Map.Height; y++)
            {
                for (var x = 0; x < this.Map.Width; x++)
                {
                    this.Visited[y, x] = this.Input[y][x] == '#';
                }
            }

            Dictionary<int, bool[,]> levels = new() { { 0, this.Visited } };

            for (var x = 0; x < 200; x++)
            {
                this.EvolveWithLevels(levels);
            }

            int totalBugs = 0;

            foreach (var kvp in levels.OrderBy(kvp => kvp.Key))
            {
                for (var y = 0; y < this.Map.Height; y++)
                {
                    for (var x = 0; x < this.Map.Width; x++)
                    {
                        if (kvp.Value[y, x])
                        {
                            totalBugs++;
                        }
                    }
                }
            }

            return totalBugs;
        }

        private static bool HasOuterEdgeBugs(bool[,] level) => Vector<int>.Outer.Any(p => level[p.Y, p.X]);

        private static bool HasInnerEdgeBugs(bool[,] level) => Vector<int>.Inner.Any(p => level[p.Y, p.X]);

        private void EvolveWithLevels(Dictionary<int, bool[,]> levels)
        {
            Dictionary<int, bool[,]> newLevels = new();

            int minLevel = levels.Keys.Min();
            int maxLevel = levels.Keys.Max();

            if (HasInnerEdgeBugs(levels[minLevel]))
            {
                levels.Add(minLevel - 1, new bool[this.Map.Height, this.Map.Width]);
            }

            if (HasOuterEdgeBugs(levels[maxLevel]))
            {
                levels.Add(maxLevel + 1, new bool[this.Map.Height, this.Map.Width]);
            }

            foreach (var kvp in levels)
            {
                int curLevel = kvp.Key;
                bool[,] map = kvp.Value;
                bool[,] newMap = new bool[this.Map.Height, this.Map.Width];

                for (var y = 0; y < map.GetLength(0); y++)
                {
                    for (var x = 0; x < map.GetLength(1); x++)
                    {
                        if (x == 2 && y == 2)
                        {
                            continue;
                        }

                        var curPoint = new Vector<int>(x, y);
                        int bugsAroundCount = curPoint.Around(0, 0, this.Map.Width - 1, this.Map.Height - 1).Count(p => levels[curLevel][p.Y, p.X]);

                        if (levels[curLevel][2, 2])
                        {
                            bugsAroundCount--;
                        }

                        int parentLevel = curLevel - 1;

                        if (x == 0)
                        {
                            if (levels.ContainsKey(parentLevel))
                            {
                                bugsAroundCount += levels[parentLevel][2, 1] == true ? 1 : 0;
                            }
                        }
                        else if (x == this.Map.Width - 1)
                        {
                            if (levels.ContainsKey(parentLevel))
                            {
                                bugsAroundCount += levels[parentLevel][2, 3] == true ? 1 : 0;
                            }
                        }

                        if (y == 0)
                        {
                            if (levels.ContainsKey(parentLevel))
                            {
                                bugsAroundCount += levels[parentLevel][1, 2] == true ? 1 : 0;
                            }
                        }
                        else if (y == this.Map.Height - 1)
                        {
                            if (levels.ContainsKey(parentLevel))
                            {
                                bugsAroundCount += levels[parentLevel][3, 2] == true ? 1 : 0;
                            }
                        }

                        int childLevel = curLevel + 1;

                        if (x == 2 && y == 1)
                        {
                            if (levels.ContainsKey(childLevel))
                            {
                                Vector<int>[] childPoints = new Vector<int>[] { new Vector<int>(0, 0), new Vector<int>(1, 0), new Vector<int>(2, 0), new Vector<int>(3, 0), new Vector<int>(4, 0) };
                                bugsAroundCount += childPoints.Count(p => levels[childLevel][p.Y, p.X]);
                            }
                        }
                        else if (x == 2 && y == 3)
                        {
                            if (levels.ContainsKey(childLevel))
                            {
                                Vector<int>[] childPoints = new Vector<int>[] { new Vector<int>(0, 4), new Vector<int>(1, 4), new Vector<int>(2, 4), new Vector<int>(3, 4), new Vector<int>(4, 4) };
                                bugsAroundCount += childPoints.Count(p => levels[childLevel][p.Y, p.X]);
                            }
                        }
                        else if (x == 1 && y == 2)
                        {
                            if (levels.ContainsKey(childLevel))
                            {
                                Vector<int>[] childPoints = new Vector<int>[] { new Vector<int>(0, 0), new Vector<int>(0, 1), new Vector<int>(0, 2), new Vector<int>(0, 3), new Vector<int>(0, 4) };
                                bugsAroundCount += childPoints.Count(p => levels[childLevel][p.Y, p.X]);
                            }
                        }
                        else if (x == 3 && y == 2)
                        {
                            if (levels.ContainsKey(childLevel))
                            {
                                Vector<int>[] childPoints = new Vector<int>[] { new Vector<int>(4, 0), new Vector<int>(4, 1), new Vector<int>(4, 2), new Vector<int>(4, 3), new Vector<int>(4, 4) };
                                bugsAroundCount += childPoints.Count(p => levels[childLevel][p.Y, p.X]);
                            }
                        }

                        if (levels[curLevel][y, x])
                        {
                            if (bugsAroundCount != 1)
                            {
                                newMap[y, x] = false;
                            }
                            else
                            {
                                newMap[y, x] = true;
                            }
                        }
                        else
                        {
                            if (bugsAroundCount == 1 || bugsAroundCount == 2)
                            {
                                newMap[y, x] = true;
                            }
                            else
                            {
                                newMap[y, x] = false;
                            }
                        }
                    }
                }

                newLevels.Add(curLevel, newMap);
            }

            foreach (var kvp in newLevels)
            {
                levels[kvp.Key] = kvp.Value;
            }
        }
    }
}
