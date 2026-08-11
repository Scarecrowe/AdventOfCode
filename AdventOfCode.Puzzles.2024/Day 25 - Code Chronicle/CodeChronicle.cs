namespace AdventOfCode.Puzzles._2024.Day_25___Code_Chronicle
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class CodeChronicle
    {
        public List<VectorArray<int, char>> Locks { get; private set; }

        public List<VectorArray<int, char>> Keys { get; private set; }

        public CodeChronicle(string[] input)
        {
            this.Locks = [];
            this.Keys = [];
            this.Parse(input);
        }

        public CodeChronicle(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private sealed record PairFrame(
            int LockIndex,
            int KeyIndex,
            List<int> LockHeights,
            List<int> KeyHeights,
            bool[] ColumnsFit,
            bool IsMatch,
            int RunningMatches,
            int PairNumber,
            int TotalPairs);

        public int Unique()
        {
            int result = 0;
            int height = 0;

            foreach (var key in this.Keys)
            {
                List<int> keyHeights = [];

                for (int x = 0; x < key.Width; x++)
                {
                    height = 0;

                    for (int y = key.Height - 1; y >= 0; y--)
                    {
                        if (key[new(x, y)] == '#')
                        {
                            height++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    keyHeights.Add(height - 1);
                }

                foreach (var @lock in this.Locks)
                {
                    List<int> lockHeights = [];

                    for (int x = 0; x < @lock.Width; x++)
                    {
                        height = 0;

                        for (int y = 0; y < @lock.Height; y++)
                        {
                            if (@lock[new(x, y)] == '#')
                            {
                                height++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        lockHeights.Add(height - 1);
                    }

                    bool isMatch = true;

                    for (int i = 0; i < keyHeights.Count; i++)
                    {
                        int keyHeight = keyHeights[i];
                        int lockHeight = lockHeights[i];

                        int total = keyHeight + lockHeight;

                        if (total > 5)
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public CodeChronicle RenderSilver(int renderEvery = 12)
        {
            return this.RenderPairs(renderEvery);
        }

        public CodeChronicle RenderGold(int renderEvery = 12)
        {
            return this.RenderPairs(renderEvery);
        }

        private void Parse(string[] input)
        {
            List<string> lines = [];
            List<VectorArray<int, char>> maps = [];

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    maps.Add(new([.. lines], c => c));
                    lines.Clear();
                    continue;
                }

                lines.Add(line);
            }

            maps.Add(new([.. lines], c => c));

            foreach (var map in maps)
            {
                bool isLock = true;

                for (int x = 0; x < map.Width; x++)
                {
                    if (map[new(x, 0)] != '#')
                    {
                        isLock = false;
                        break;
                    }
                }

                if (isLock)
                {
                    this.Locks.Add(map);
                }
                else
                {
                    this.Keys.Add(map);
                }
            }
        }

        private CodeChronicle RenderPairs(int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PairFrame> frames = [];
            int runningMatches = 0;
            int pairNumber = 0;
            int totalPairs = this.Locks.Count * this.Keys.Count;

            for (int keyIndex = 0; keyIndex < this.Keys.Count; keyIndex++)
            {
                VectorArray<int, char> key = this.Keys[keyIndex];
                List<int> keyHeights = this.GetKeyHeights(key);

                for (int lockIndex = 0; lockIndex < this.Locks.Count; lockIndex++)
                {
                    VectorArray<int, char> @lock = this.Locks[lockIndex];
                    List<int> lockHeights = this.GetLockHeights(@lock);
                    bool[] columnsFit = new bool[keyHeights.Count];
                    bool isMatch = true;

                    for (int i = 0; i < keyHeights.Count; i++)
                    {
                        columnsFit[i] = keyHeights[i] + lockHeights[i] <= 5;

                        if (!columnsFit[i])
                        {
                            isMatch = false;
                        }
                    }

                    pairNumber++;

                    if (isMatch)
                    {
                        runningMatches++;
                    }

                    if (pairNumber % renderEvery == 0 || isMatch || pairNumber == 1 || pairNumber == totalPairs)
                    {
                        frames.Add(new(
                            lockIndex,
                            keyIndex,
                            lockHeights,
                            keyHeights,
                            columnsFit,
                            isMatch,
                            runningMatches,
                            pairNumber,
                            totalPairs));
                    }
                }
            }

            PairFrame final = frames.Last();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(final);
            }

            foreach (PairFrame frame in frames)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(frame)));
            }

            return this;
        }

        private List<int> GetLockHeights(VectorArray<int, char> @lock)
        {
            List<int> heights = [];

            for (int x = 0; x < @lock.Width; x++)
            {
                int height = 0;

                for (int y = 0; y < @lock.Height; y++)
                {
                    if (@lock[new(x, y)] == '#')
                    {
                        height++;
                    }
                    else
                    {
                        break;
                    }
                }

                heights.Add(height - 1);
            }

            return heights;
        }

        private List<int> GetKeyHeights(VectorArray<int, char> key)
        {
            List<int> heights = [];

            for (int x = 0; x < key.Width; x++)
            {
                int height = 0;

                for (int y = key.Height - 1; y >= 0; y--)
                {
                    if (key[new(x, y)] == '#')
                    {
                        height++;
                    }
                    else
                    {
                        break;
                    }
                }

                heights.Add(height - 1);
            }

            return heights;
        }

        private string[] BuildFrame(PairFrame frame)
        {
            List<string> result = [];
            VectorArray<int, char> @lock = this.Locks[frame.LockIndex];
            VectorArray<int, char> key = this.Keys[frame.KeyIndex];
            string verdict = frame.IsMatch ? "FIT" : "OVERLAP";
            string progress = $"PAIR {frame.PairNumber:000000}/{frame.TotalPairs:000000}";
            string count = $"MATCHES {frame.RunningMatches:0000}";

            result.Add("CODE CHRONICLE // FIVE PIN TUMBLERS");
            result.Add($"{progress} // LOCK {frame.LockIndex:0000} // KEY {frame.KeyIndex:0000} // {count}");
            result.Add(string.Empty);
            result.Add("LOCK       KEY        TOTALS");

            for (int y = 0; y < @lock.Height; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < @lock.Width; x++)
                {
                    row.Append(@lock[new(x, y)]);
                }

                row.Append("    ");

                for (int x = 0; x < key.Width; x++)
                {
                    row.Append(key[new(x, y)]);
                }

                row.Append("      ");

                if (y < frame.LockHeights.Count)
                {
                    int total = frame.LockHeights[y] + frame.KeyHeights[y];
                    row.Append(total <= 5 ? 'o' : 'x');
                    row.Append(' ');
                    row.Append(total);
                    row.Append("/5");
                }

                result.Add(row.ToString());
            }

            result.Add(string.Empty);
            result.Add($"LOCK HEIGHTS  {FormatHeights(frame.LockHeights)}");
            result.Add($"KEY HEIGHTS   {FormatHeights(frame.KeyHeights)}");
            result.Add($"COLUMN TEST   {FormatColumnTest(frame.ColumnsFit)}");
            result.Add(string.Empty);
            result.Add(frame.IsMatch
                ? "RESULT        KEY COULD FIT THIS LOCK"
                : "RESULT        OVERLAP DETECTED");
            result.Add($"ANSWER        {frame.RunningMatches:0000}");

            return PadFrame([.. result], 39, 18);
        }

        private static string FormatHeights(List<int> heights)
        {
            return string.Join(' ', heights.Select(h => h.ToString()));
        }

        private static string FormatColumnTest(bool[] columnsFit)
        {
            return string.Join(' ', columnsFit.Select(fit => fit ? "OK" : "XX"));
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
