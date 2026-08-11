namespace AdventOfCode.Puzzles._2019.Day_24___Planet_of_Discord
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class PlanetOfDiscord
    {
        public PlanetOfDiscord(string[] input)
        {
            this.Input = input;
            this.Map = new(input, (c) => c);
            this.Cache = new();
            this.Visited = new bool[0, 0];
        }

        public PlanetOfDiscord(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

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

        public PlanetOfDiscord RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<string> seen = [];

            int minute = 0;
            seen.Add(this.Map.Flatten().Join());

            frames.Add(this.BuildSingleLevelFrame(
                $"PLANET OF DISCORD // MINUTE {minute:000} // BIODIVERSITY {this.GetBiodiversity(this.Map):000000000}",
                this.Map));

            while (true)
            {
                minute++;

                VectorArray<int, char> next = this.EvolveSingleLevel(this.Map);
                string key = next.Flatten().Join();

                this.Map = next;

                if (minute % renderEvery == 0 || seen.Contains(key))
                {
                    frames.Add(this.BuildSingleLevelFrame(
                        seen.Contains(key)
                            ? $"FIRST REPEATED LAYOUT // MINUTE {minute:000} // BIODIVERSITY {this.GetBiodiversity(this.Map):000000000}"
                            : $"PLANET OF DISCORD // MINUTE {minute:000} // BIODIVERSITY {this.GetBiodiversity(this.Map):000000000}",
                        this.Map));
                }

                if (seen.Contains(key))
                {
                    frames.Add(this.BuildSingleLevelFrame(
                           $"FIRST REPEATED LAYOUT // BIODIVERSITY {this.GetBiodiversity(this.Map):000000000}",
                           this.Map));

                    break;
                }

                seen.Add(key);
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public PlanetOfDiscord RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Dictionary<int, bool[,]> levels = this.CreateInitialLevels();

            List<string[]> frames = [];

            frames.Add(this.BuildRecursiveFrame(
                $"RECURSIVE PLANET OF DISCORD // MINUTE 000 // BUGS {this.CountBugs(levels):0000}",
                levels));

            for (int minute = 1; minute <= 200; minute++)
            {
                this.EvolveWithLevels(levels);

                if (minute % renderEvery == 0 || minute == 200)
                {
                    frames.Add(this.BuildRecursiveFrame(
                        $"RECURSIVE PLANET OF DISCORD // MINUTE {minute:000} // BUGS {this.CountBugs(levels):0000}",
                        levels,
                        visibleRadius: 6));
                }
            }

            frames.Add(this.BuildRecursiveFrame(
                    $"AFTER 200 MINUTES // BUGS {this.CountBugs(levels):0000}",
                    levels));

            this.RenderPaddedFrames(frames);

            return this;
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

        private VectorArray<int, char> EvolveSingleLevel(VectorArray<int, char> map)
        {
            VectorArray<int, char> next = new(map.Width, map.Height);

            foreach (VectorCell<int, char> cell in map.AxisEnumerator())
            {
                int bugs = map.AdjacentCardinal(cell.Point).Count(c => c.Value == '#');

                if (cell.Value == '#')
                {
                    next[cell.Point] = bugs == 1 ? '#' : '.';
                }
                else
                {
                    next[cell.Point] = bugs is 1 or 2 ? '#' : '.';
                }
            }

            return next;
        }

        private long GetBiodiversity(VectorArray<int, char> map)
        {
            long result = 0;
            string key = map.Flatten().Join();

            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == '#')
                {
                    result += 1L << i;
                }
            }

            return result;
        }

        private Dictionary<int, bool[,]> CreateInitialLevels()
        {
            bool[,] level = new bool[this.Map.Height, this.Map.Width];

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    level[y, x] = this.Input[y][x] == '#';
                }
            }

            return new Dictionary<int, bool[,]>
    {
        { 0, level }
    };
        }

        private int CountBugs(Dictionary<int, bool[,]> levels)
        {
            int total = 0;

            foreach (bool[,] level in levels.Values)
            {
                for (int y = 0; y < this.Map.Height; y++)
                {
                    for (int x = 0; x < this.Map.Width; x++)
                    {
                        if (level[y, x])
                        {
                            total++;
                        }
                    }
                }
            }

            return total;
        }

        private string[] BuildSingleLevelFrame(string title, VectorArray<int, char> map)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < map.Width; x++)
                {
                    sb.Append(map[y, x]);
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildRecursiveFrame(
            string title,
            Dictionary<int, bool[,]> levels,
            int visibleRadius = 5)
        {
            List<string> result = [];

            int bugCount = this.CountBugs(levels);
            int minLevel = levels.Keys.Min();
            int maxLevel = levels.Keys.Max();

            result.Add(title);
            result.Add($"VISIBLE LEVELS {-visibleRadius:+000;-000;000} TO {visibleRadius:+000;-000;000} // ACTIVE LEVELS {minLevel:+000;-000;000} TO {maxLevel:+000;-000;000}");
            result.Add(string.Empty);

            for (int level = -visibleRadius; level <= visibleRadius; level++)
            {
                bool[,] map = levels.TryGetValue(level, out bool[,]? existing)
                    ? existing
                    : new bool[this.Map.Height, this.Map.Width];

                result.Add($"LEVEL {level:+000;-000;000}");

                for (int y = 0; y < this.Map.Height; y++)
                {
                    StringBuilder sb = new();

                    for (int x = 0; x < this.Map.Width; x++)
                    {
                        if (x == 2 && y == 2)
                        {
                            sb.Append('?');
                        }
                        else
                        {
                            sb.Append(map[y, x] ? '#' : '.');
                        }
                    }

                    result.Add(sb.ToString());
                }

                result.Add(string.Empty);
            }

            result.Add($"TOTAL BUGS {bugCount:0000}");

            return [.. result];
        }

        private static bool LevelHasBugs(bool[,] level)
        {
            for (int y = 0; y < level.GetLength(0); y++)
            {
                for (int x = 0; x < level.GetLength(1); x++)
                {
                    if (level[y, x])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}
