namespace AdventOfCode.Puzzles._2024.Day_08___Resonant_Collinearity
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class ResonantCollinearity
    {
        public VectorArray<int, char> Map { get; private set; }

        public Dictionary<char, HashSet<Vector<int>>> Antennas { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct PairFrame(
            char Frequency,
            Vector<int> PointA,
            Vector<int> PointB,
            HashSet<Vector<int>> AntiNodes,
            int PairIndex,
            int PairTotal);

        public ResonantCollinearity(string[] input)
        {
            this.Map = new(input, c => c);
            this.Antennas = this.GetAntennas();
        }

        public ResonantCollinearity(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        private Dictionary<char, HashSet<Vector<int>>> GetAntennas()
        {
            Dictionary<char, HashSet<Vector<int>>> result = [];

            foreach (var cell in this.Map.AxisEnumerator().Where(x => x.Value != '.'))
            {
                if (!result.ContainsKey(cell.Value))
                {
                    result[cell.Value] = new();
                }

                result[cell.Value].Add(cell.Point);
            }

            return result;
        }

        private static List<Vector<int>> PathToAntenna(Vector<int> start, Vector<int> end)
        {
            List<Vector<int>> path = new();

            int currentX = start.X;
            int currentY = start.Y;

            while (currentX != end.X)
            {
                path.Add(new(currentX, currentY));
                currentX += (end.X > currentX) ? 1 : -1;
            }

            while (currentY != end.Y)
            {
                path.Add(new(currentX, currentY));
                currentY += (end.Y > currentY) ? 1 : -1;
            }

            path.Add(new(currentX, currentY));

            return path;
        }

        private static List<Vector<int>> PathToNode(List<Vector<int>> path)
        {
            var result = new List<Vector<int>>();
            Vector<int> current = new(path[0].X, path[0].Y);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector<int> start = new(path[i].X, path[i].Y);
                Vector<int> end = new(path[i + 1].X, path[i + 1].Y);
                Vector<int> delta = new(start - end);

                current += delta;
                result.Add(current);
            }

            return result;
        }

        private static List<Vector<int>> PathDeltas(List<Vector<int>> path)
        {
            List<Vector<int>> result = [];

            for (int i = 0; i < path.Count - 1; i++)
            {
                result.Add(new(path[i] - path[i + 1]));
            }

            return result;
        }

        private void UniqueNodes(HashSet<Vector<int>> result, Vector<int> pointA, Vector<int> pointB)
        {
            List<Vector<int>> path = PathToAntenna(pointA, pointB);
            Vector<int> nodePoint = PathToNode(path).Last();

            if (!result.Contains(nodePoint)
                && this.Map.IsVectorInRange(nodePoint))
            {
                result.Add(nodePoint);
            }

            path.Reverse();
            nodePoint = PathToNode(path).Last();

            if (!result.Contains(nodePoint)
                && this.Map.IsVectorInRange(nodePoint))
            {
                result.Add(nodePoint);
            }
        }

        private void Resonate(HashSet<Vector<int>> result, Vector<int> pointA, Vector<int> pointB)
        {
            List<Vector<int>> path = PathToAntenna(pointA, pointB);
            List<Vector<int>> deltas = PathDeltas(path);
            Vector<int> current = pointB;

            while (true)
            {
                foreach (var delta in deltas)
                {
                    current += delta;
                }

                if (this.Map.IsVectorInRange(current))
                {
                    if (!result.Contains(current))
                    {
                        result.Add(current);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public HashSet<Vector<int>> AntiNodes(bool resonate = false)
        {
            HashSet<Vector<int>> result = new();

            foreach (var antenna in this.Antennas)
            {
                foreach (var pointA in antenna.Value)
                {
                    foreach (var pointB in antenna.Value)
                    {
                        if (pointA != pointB)
                        {
                            if (!resonate)
                            {
                                this.UniqueNodes(result, pointA, pointB);
                            }
                            else
                            {
                                this.Resonate(result, pointA, pointB);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public ResonantCollinearity RenderSilver(int renderEvery = 1)
        {
            return this.RenderAntiNodes(resonate: false, renderEvery);
        }

        public ResonantCollinearity RenderGold(int renderEvery = 1)
        {
            return this.RenderAntiNodes(resonate: true, renderEvery);
        }

        private ResonantCollinearity RenderAntiNodes(bool resonate, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PairFrame> pairFrames = this.BuildPairFrames(resonate);
            List<string[]> frames = [];
            Vector<int> noPoint = new(-1, -1);

            frames.Add(this.BuildFrame(
                frequency: ' ',
                pointA: noPoint,
                pointB: noPoint,
                antiNodes: [],
                title: resonate
                    ? "RESONANT COLLINEARITY // HARMONICS START"
                    : "RESONANT COLLINEARITY // ANTINODES START",
                subtitle: $"ANTENNAS {this.Antennas.Sum(x => x.Value.Count):000} // FREQUENCIES {this.Antennas.Count:000}"));

            for (int i = 0; i < pairFrames.Count; i++)
            {
                PairFrame frame = pairFrames[i];

                if (i % renderEvery == 0 || i == pairFrames.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        frame.Frequency,
                        frame.PointA,
                        frame.PointB,
                        frame.AntiNodes,
                        resonate
                            ? $"RESONANT HARMONICS // FREQUENCY {frame.Frequency}"
                            : $"RESONANT COLLINEARITY // FREQUENCY {frame.Frequency}",
                        $"PAIR {frame.PairIndex:0000}/{frame.PairTotal:0000} // ANTINODES {frame.AntiNodes.Count:0000}"));
                }
            }

            if (pairFrames.Count > 0)
            {
                PairFrame last = pairFrames.Last();
                string[] finalFrame = this.BuildFrame(
                    frequency: ' ',
                    pointA: noPoint,
                    pointB: noPoint,
                    antiNodes: last.AntiNodes,
                    title: resonate
                        ? "HARMONICS COMPLETE"
                        : "ANTINODES COMPLETE",
                    subtitle: $"UNIQUE ANTINODES {last.AntiNodes.Count:0000}");

                for (int i = 0; i < 24; i++)
                {
                    frames.Add(finalFrame);
                }
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<PairFrame> BuildPairFrames(bool resonate)
        {
            List<(char Frequency, Vector<int> PointA, Vector<int> PointB)> pairs = [];

            foreach (var antenna in this.Antennas.OrderBy(x => x.Key))
            {
                foreach (Vector<int> pointA in antenna.Value.OrderBy(p => p.Y).ThenBy(p => p.X))
                {
                    foreach (Vector<int> pointB in antenna.Value.OrderBy(p => p.Y).ThenBy(p => p.X))
                    {
                        if (pointA != pointB)
                        {
                            pairs.Add((antenna.Key, pointA, pointB));
                        }
                    }
                }
            }

            List<PairFrame> result = [];
            HashSet<Vector<int>> antiNodes = [];

            for (int i = 0; i < pairs.Count; i++)
            {
                (char frequency, Vector<int> pointA, Vector<int> pointB) = pairs[i];

                if (!resonate)
                {
                    this.UniqueNodes(antiNodes, pointA, pointB);
                }
                else
                {
                    this.Resonate(antiNodes, pointA, pointB);
                }

                result.Add(new(
                    frequency,
                    pointA,
                    pointB,
                    [.. antiNodes],
                    i + 1,
                    pairs.Count));
            }

            return result;
        }

        private string[] BuildFrame(
            char frequency,
            Vector<int> pointA,
            Vector<int> pointB,
            HashSet<Vector<int>> antiNodes,
            string title,
            string subtitle)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(subtitle);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Map[y, x];

                    if (point == pointA)
                    {
                        sb.Append('1');
                    }
                    else if (point == pointB)
                    {
                        sb.Append('2');
                    }
                    else if (antiNodes.Contains(point) && value != '.')
                    {
                        sb.Append('@');
                    }
                    else if (antiNodes.Contains(point))
                    {
                        sb.Append('#');
                    }
                    else if (value != '.')
                    {
                        sb.Append(value == frequency ? '*' : value);
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
