namespace AdventOfCode.Puzzles._2023.Day_11___Cosmic_Expansion
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class CosmicExpansion
    {
        public CosmicExpansion(string[] input)
        {
            this.Input = input;
            this.Width = input[0].Length;
            this.Height = input.Length;

            this.Galaxies = new VectorDictionary<int, char>(input, c => c)
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

        public CosmicExpansion(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public List<Vector<int>> Galaxies { get; }

        public List<int> Cols { get; }

        public List<int> Rows { get; }

        private string[] Input { get; }

        private int Width { get; }

        private int Height { get; }

        private readonly record struct GalaxyPair(Vector<int> A, Vector<int> B, long Distance, long RunningTotal);

        public long SumOfShortestPath(long amount)
        {
            long result = 0;

            foreach (var (pointA, pointB) in this.Galaxies.PairEnumerator())
            {
                result += this.Distance(pointA, pointB, amount);
            }

            return result;
        }

        public CosmicExpansion RenderSilver(int renderEvery = 1)
            => this.RenderExpansion(2, "COSMIC EXPANSION", renderEvery);

        public CosmicExpansion RenderGold(int renderEvery = 24)
            => this.RenderExpansion(1_000_000, "DEEP COSMIC EXPANSION", renderEvery);

        private CosmicExpansion RenderExpansion(long amount, string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<GalaxyPair> pairs = this.GetPairs(amount);
            HashSet<(long X, long Y)> completed = [];

            for (int i = 0; i < pairs.Count; i++)
            {
                GalaxyPair pair = pairs[i];

                foreach ((long X, long Y) point in this.GetPath(pair.A, pair.B))
                {
                    completed.Add(point);
                }

                if (i % renderEvery != 0 && i != pairs.Count - 1)
                {
                    continue;
                }

                this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                    title,
                    amount,
                    pair,
                    completed,
                    i + 1,
                    pairs.Count)));
            }

            return this;
        }

        private List<GalaxyPair> GetPairs(long amount)
        {
            List<GalaxyPair> result = [];
            long total = 0;

            foreach (var (a, b) in this.Galaxies.PairEnumerator())
            {
                long distance = this.Distance(a, b, amount);
                total += distance;

                result.Add(new(a, b, distance, total));
            }

            return result;
        }

        private long Distance(Vector<int> a, Vector<int> b, long amount)
        {
            return a.Distance(b)
                + ((amount - 1) * this.Cols.Count(c => c > Math.Min(a.X, b.X) && c < Math.Max(a.X, b.X)))
                + ((amount - 1) * this.Rows.Count(r => r > Math.Min(a.Y, b.Y) && r < Math.Max(a.Y, b.Y)));
        }

        private (long X, long Y) VisualPoint(Vector<int> point)
        {
            long x = point.X + this.Cols.Count(c => c < point.X);
            long y = point.Y + this.Rows.Count(r => r < point.Y);

            return (x, y);
        }

        private IEnumerable<(long X, long Y)> GetPath(Vector<int> a, Vector<int> b)
        {
            (long ax, long ay) = this.VisualPoint(a);
            (long bx, long by) = this.VisualPoint(b);

            long xStep = ax <= bx ? 1 : -1;
            long yStep = ay <= by ? 1 : -1;

            for (long x = ax; x != bx; x += xStep)
            {
                yield return (x, ay);
            }

            for (long y = ay; y != by; y += yStep)
            {
                yield return (bx, y);
            }

            yield return (bx, by);
        }

        private string[] BuildFrame(
            string title,
            long amount,
            GalaxyPair pair,
            HashSet<(long X, long Y)> completed,
            int pairNumber,
            int pairCount)
        {
            int visualWidth = this.Width + this.Cols.Count;
            int visualHeight = this.Height + this.Rows.Count;

            List<string> result = [];

            result.Add($"{title} // EXPANSION x{amount:N0}");
            result.Add($"PAIR {pairNumber:00000}/{pairCount:00000} // DISTANCE {pair.Distance:N0} // TOTAL {pair.RunningTotal:N0}");
            result.Add(string.Empty);

            Dictionary<(long X, long Y), char> galaxies = this.Galaxies
                .Select((p, i) => (Point: this.VisualPoint(p), Label: (char)('A' + (i % 26))))
                .ToDictionary(x => x.Point, x => x.Label);

            HashSet<(long X, long Y)> currentPath = this.GetPath(pair.A, pair.B).ToHashSet();
            (long AX, long AY) = this.VisualPoint(pair.A);
            (long BX, long BY) = this.VisualPoint(pair.B);

            for (int y = 0; y < visualHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < visualWidth; x++)
                {
                    if (x == AX && y == AY)
                    {
                        sb.Append('@');
                    }
                    else if (x == BX && y == BY)
                    {
                        sb.Append('$');
                    }
                    else if (galaxies.TryGetValue((x, y), out char label))
                    {
                        sb.Append(label);
                    }
                    else if (currentPath.Contains((x, y)))
                    {
                        sb.Append('*');
                    }
                    else if (completed.Contains((x, y)))
                    {
                        sb.Append('~');
                    }
                    else if (this.IsExpandedColumn(x) || this.IsExpandedRow(y))
                    {
                        sb.Append('+');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private bool IsExpandedColumn(int visualX)
        {
            int offset = 0;

            foreach (int col in this.Cols)
            {
                if (visualX == col + offset + 1)
                {
                    return true;
                }

                offset++;
            }

            return false;
        }

        private bool IsExpandedRow(int visualY)
        {
            int offset = 0;

            foreach (int row in this.Rows)
            {
                if (visualY == row + offset + 1)
                {
                    return true;
                }

                offset++;
            }

            return false;
        }
    }
}