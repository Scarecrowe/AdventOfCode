namespace AdventOfCode.Puzzles._2021.Day_09___Smoke_Basin
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class SmokeBasin
    {
        private const int ViewWidth = 120;
        private const int ViewHeight = 60;
        private const int TitleRows = 2;

        public SmokeBasin(string[] input) => this.Map = ParseInput(input);

        public SmokeBasin(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, int> Map { get; }

        private int ViewX { get; set; }

        private int ViewY { get; set; }

        public int SumOfRiskLevels() => this.Lowest().Sum(point => this.Map[point] + 1);

        public long SumOfBasin()
        {
            long total = 0;

            foreach (HashSet<Vector<int>> basin in this.Basins().OrderByDescending(x => x.Count).Take(3))
            {
                total = total == 0 ? basin.Count : total * basin.Count;
            }

            return total;
        }

        public SmokeBasin RenderSilver(int renderEvery = 8)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            this.ViewX = 0;
            this.ViewY = 0;

            HashSet<Vector<int>> lows = [];
            int scanned = 0;

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    scanned++;

                    if (this.IsLowest(point))
                    {
                        lows.Add(point);
                    }

                    if (scanned % renderEvery == 0)
                    {
                        this.RenderFrame(
                            current: point,
                            lows: lows,
                            basin: [],
                            topBasins: [],
                            title: $"SMOKE BASIN // SCANNING // RISK {lows.Sum(p => this.Map[p] + 1):0000}",
                            focus: point);
                    }
                }
            }

            Vector<int> finalFocus = lows.LastOrDefault();

            for (int i = 0; i < 24; i++)
            {
                this.RenderFrame(
                    current: finalFocus,
                    lows: lows,
                    basin: [],
                    topBasins: [],
                    title: $"LOW POINTS FOUND // RISK {this.SumOfRiskLevels():0000}",
                    focus: finalFocus);
            }

            return this;
        }

        public SmokeBasin RenderGold(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            this.ViewX = 0;
            this.ViewY = 0;

            HashSet<Vector<int>> lows = this.Lowest();
            List<HashSet<Vector<int>>> finishedBasins = [];
            int basinNumber = 0;

            foreach (Vector<int> low in lows)
            {
                basinNumber++;

                HashSet<Vector<int>> basin = [];
                Queue<Vector<int>> queue = new();

                queue.Enqueue(low);

                int step = 0;

                while (queue.Count > 0)
                {
                    Vector<int> point = queue.Dequeue();

                    if (basin.Contains(point) || this.Map[point] == 9)
                    {
                        continue;
                    }

                    basin.Add(point);

                    foreach (VectorCell<int, int> adjacent in this.Map.AdjacentCardinal(point))
                    {
                        if (!basin.Contains(adjacent.Point) && adjacent.Value != 9)
                        {
                            queue.Enqueue(adjacent.Point);
                        }
                    }

                    step++;

                    if (step % renderEvery == 0)
                    {
                        this.RenderFrame(
                            current: point,
                            lows: lows,
                            basin: basin,
                            topBasins: finishedBasins,
                            title: $"BASIN {basinNumber:00} // GROWING // SIZE {basin.Count:0000}",
                            focus: low);
                    }
                }

                finishedBasins.Add(basin);

                this.RenderFrame(
                    current: low,
                    lows: lows,
                    basin: basin,
                    topBasins: finishedBasins,
                    title: $"BASIN {basinNumber:00} COMPLETE // SIZE {basin.Count:0000}",
                    focus: low);
            }

            List<HashSet<Vector<int>>> topThree = finishedBasins
                .OrderByDescending(x => x.Count)
                .Take(3)
                .ToList();

            long score = topThree.Aggregate(1L, (total, basin) => total * basin.Count);
            Vector<int> finalFocus = topThree.First().First();

            for (int i = 0; i < 32; i++)
            {
                this.RenderFrame(
                    current: finalFocus,
                    lows: lows,
                    basin: [],
                    topBasins: topThree,
                    title: $"THREE LARGEST BASINS // PRODUCT {score}",
                    focus: finalFocus);
            }

            return this;
        }

        private static VectorArray<int, int> ParseInput(string[] input) => new(input, c => $"{c}".ToInt());

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
            HashSet<Vector<int>> lowest = [];

            for (int y = 0; y < this.Map.Height; ++y)
            {
                for (int x = 0; x < this.Map.Width; ++x)
                {
                    Vector<int> point = new(x, y);

                    if (this.IsLowest(point))
                    {
                        lowest.Add(point);
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

        private HashSet<HashSet<Vector<int>>> Basins()
            => this.Lowest().Select(x => this.Basin(x, [])).ToHashSet();

        private void RenderFrame(
            Vector<int> current,
            HashSet<Vector<int>> lows,
            HashSet<Vector<int>> basin,
            List<HashSet<Vector<int>>> topBasins,
            string title,
            Vector<int> focus)
        {
            string[] frame = this.BuildFrame(current, lows, basin, topBasins, title);

            if (this.Map.Width > ViewWidth || this.Map.Height + TitleRows > ViewHeight)
            {
                frame = this.CropFrameSmooth(frame, focus, ViewWidth, ViewHeight);
            }

            this.Renderer?.RenderFrame(new Frame(frame));
        }

        private string[] CropFrameSmooth(string[] frame, Vector<int> focus, int viewWidth, int viewHeight)
        {
            int mapWidth = frame.Skip(TitleRows).Max(x => x.Length);
            int mapHeight = frame.Length - TitleRows;
            int visibleMapHeight = viewHeight - TitleRows;

            int targetX = Math.Clamp(
                focus.X - viewWidth / 2,
                0,
                Math.Max(0, mapWidth - viewWidth));

            int targetY = Math.Clamp(
                focus.Y - visibleMapHeight / 2,
                0,
                Math.Max(0, mapHeight - visibleMapHeight));

            this.ViewX = StepTowards(this.ViewX, targetX, 4);
            this.ViewY = StepTowards(this.ViewY, targetY, 2);

            List<string> result = [];

            result.Add(frame[0].PadRight(viewWidth)[..viewWidth]);
            result.Add(frame[1].PadRight(viewWidth)[..viewWidth]);

            for (int y = 0; y < visibleMapHeight; y++)
            {
                string row = frame[this.ViewY + TitleRows + y].PadRight(mapWidth);
                result.Add(row.Substring(this.ViewX, Math.Min(viewWidth, row.Length - this.ViewX)).PadRight(viewWidth));
            }

            return [.. result];
        }

        private static int StepTowards(int current, int target, int maxStep)
        {
            if (current < target)
            {
                return Math.Min(current + maxStep, target);
            }

            if (current > target)
            {
                return Math.Max(current - maxStep, target);
            }

            return current;
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> lows,
            HashSet<Vector<int>> basin,
            List<HashSet<Vector<int>>> topBasins,
            string title)
        {
            List<string> result = [];

            result.Add(title.PadRight(this.Map.Width));
            result.Add(new string(' ', this.Map.Width));

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    int value = this.Map[point];

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (topBasins.Any(b => b.Contains(point)))
                    {
                        sb.Append('▓');
                    }
                    else if (basin.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else if (lows.Contains(point))
                    {
                        sb.Append('*');
                    }
                    else if (value == 9)
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append((char)('0' + value));
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static string[] CropFrame(string[] frame, Vector<int> focus, int viewWidth, int viewHeight)
        {
            int mapWidth = frame.Skip(TitleRows).Max(x => x.Length);
            int mapHeight = frame.Length - TitleRows;

            int visibleMapHeight = viewHeight - TitleRows;

            int startX = Math.Clamp(
                focus.X - viewWidth / 2,
                0,
                Math.Max(0, mapWidth - viewWidth));

            int startY = Math.Clamp(
                focus.Y - visibleMapHeight / 2,
                0,
                Math.Max(0, mapHeight - visibleMapHeight));

            List<string> result = [];

            result.Add(frame[0].PadRight(viewWidth)[..viewWidth]);
            result.Add(frame[1].PadRight(viewWidth)[..viewWidth]);

            for (int y = 0; y < visibleMapHeight; y++)
            {
                string row = frame[startY + TitleRows + y].PadRight(mapWidth);
                result.Add(row.Substring(startX, Math.Min(viewWidth, row.Length - startX)).PadRight(viewWidth));
            }

            return [.. result];
        }
    }
}