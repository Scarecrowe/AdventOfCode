namespace AdventOfCode.Puzzles._2023.Day_18___Lavaduct_Lagoon
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Globalization;
    using System.Text;

    public class LavaductLagoon(string[] input)
    {
        private HashSet<(long x, long y)> smallTrench = new HashSet<(long x, long y)>();

        private Dictionary<long, List<(long start, long end)>> rowIntervals = new Dictionary<long, List<(long start, long end)>>();

        private List<(Direction Dir, long Distance)> instructions = InstructionParser.ParseNormalInstructions(input);

        private List<(Direction Dir, long Distance)> hexInstructions = InstructionParser.ParseHexInstructions(input);

        public LavaductLagoon(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public LavaductLagoon DigSmall(List<(Direction Dir, long Distance)> instructions)
        {
            long x = 0, y = 0;
            smallTrench.Add((x, y));

            foreach (var inst in instructions)
            {
                for (long i = 0; i < inst.Distance; i++)
                {
                    switch (inst.Dir)
                    {
                        case Direction.R: x++; break;
                        case Direction.L: x--; break;
                        case Direction.U: y++; break;
                        case Direction.D: y--; break;
                    }
                    smallTrench.Add((x, y));
                }
            }

            return this;
        }

        public LavaductLagoon DigLarge(List<(Direction Dir, long Distance)> instructions)
        {
            long x = 0, y = 0;
            AddInterval(y, x, x);

            foreach (var inst in instructions)
            {
                switch (inst.Dir)
                {
                    case Direction.R:
                        AddInterval(y, x + 1, x + inst.Distance);
                        x += inst.Distance;
                        break;
                    case Direction.L:
                        AddInterval(y, x - inst.Distance, x - 1);
                        x -= inst.Distance;
                        break;
                    case Direction.U:
                        for (long dy = 1; dy <= inst.Distance; dy++)
                            AddInterval(y + dy, x, x);
                        y += inst.Distance;
                        break;
                    case Direction.D:
                        for (long dy = 1; dy <= inst.Distance; dy++)
                            AddInterval(y - dy, x, x);
                        y -= inst.Distance;
                        break;
                }
            }

            return this;
        }

        private void AddInterval(long y, long start, long end)
        {
            if (!rowIntervals.ContainsKey(y))
            {
                rowIntervals[y] = new List<(long start, long end)>();
            }

            if (start > end) (start, end) = (end, start);
            {
                rowIntervals[y].Add((start, end));
            }
        }

        private List<(long start, long end)> MergeIntervals(List<(long start, long end)> intervals)
        {
            if (intervals.Count == 0)
            {
                return new List<(long start, long end)>();
            }

            intervals.Sort((a, b) => a.start.CompareTo(b.start));
            var merged = new List<(long start, long end)>();
            var current = intervals[0];

            for (int i = 1; i < intervals.Count; i++)
            {
                if (intervals[i].start <= current.end + 1)
                {
                    current.end = Math.Max(current.end, intervals[i].end);
                }
                else
                {
                    merged.Add(current);
                    current = intervals[i];
                }
            }

            merged.Add(current);

            return merged;
        }

        public long ComputeSmallVolume()
        {
            long minX = smallTrench.Min(p => p.x) - 1;
            long maxX = smallTrench.Max(p => p.x) + 1;
            long minY = smallTrench.Min(p => p.y) - 1;
            long maxY = smallTrench.Max(p => p.y) + 1;

            var visited = new HashSet<(long x, long y)>();
            var queue = new Queue<(long x, long y)>();
            queue.Enqueue((minX, minY));
            visited.Add((minX, minY));

            var dirs = new (long dx, long dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

            var outside = new HashSet<(long x, long y)>();

            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                outside.Add(pos);

                foreach (var d in dirs)
                {
                    var next = (pos.x + d.dx, pos.y + d.dy);

                    if (next.Item1 < minX || next.Item1 > maxX || next.Item2 < minY || next.Item2 > maxY)
                    {
                        continue;
                    }

                    if (smallTrench.Contains(next))
                    {
                        continue;
                    }

                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            long total = 0;

            for (long x = minX + 1; x < maxX; x++)
            {
                for (long y = minY + 1; y < maxY; y++)
                {
                    if (!smallTrench.Contains((x, y)) && !outside.Contains((x, y)))
                    {
                        total++;
                    }
                }
            }

            return total + smallTrench.Count;
        }

        public long ComputeLargeVolume()
        {
            long total = 0;
            foreach (var kvp in rowIntervals)
            {
                var intervals = MergeIntervals(kvp.Value);
                foreach (var (start, end) in intervals)
                {
                    total += end - start + 1;
                }
            }
            return total;
        }

        public long Small() => DigSmall(this.instructions).ComputeSmallVolume();

        public long Large() => DigLarge(this.hexInstructions).ComputeLargeVolume();

        public LavaductLagoon RenderSilver(int holdFrames = 18)
            => this.RenderLagoon(this.instructions, "LAVADUCT LAGOON", this.Small(), holdFrames);

        public LavaductLagoon RenderGold(int holdFrames = 18)
            => this.RenderLagoon(this.hexInstructions, "TRUE HEX LAGOON", this.Large(), holdFrames);

        private LavaductLagoon RenderLagoon(
            List<(Direction Dir, long Distance)> plan,
            string title,
            long volume,
            int holdFrames)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            LagoonProjection projection = this.BuildProjection(plan, width: 118, height: 38);
            List<string[]> frames = [];
            char[,] canvas = CreateCanvas(projection.Width, projection.Height, '.');
            List<ScreenSegment> segments = this.ProjectSegments(plan, projection);

            for (int i = 0; i < segments.Count; i++)
            {
                ScreenSegment segment = segments[i];
                DrawLine(canvas, segment.Start, segment.End, segment.Paint);
                DrawPoint(canvas, segment.End, '@');

                frames.Add(this.BuildFrame(
                    canvas,
                    title,
                    $"DIG {i + 1:000}/{segments.Count:000}  DIR {segment.Dir}  DIST {segment.Distance}",
                    $"BOUNDS {projection.WorldWidth} x {projection.WorldHeight}  TARGET {volume}"));

                DrawPoint(canvas, segment.End, segment.Paint);
            }

            char[,] filled = CloneCanvas(canvas);
            List<ScreenPoint> polygon = segments.Select(s => s.Start).ToList();

            for (int y = 0; y < projection.Height; y++)
            {
                for (int x = 0; x < projection.Width; x++)
                {
                    if (filled[y, x] == '.' && IsInsidePolygon(x, y, polygon))
                    {
                        filled[y, x] = '~';
                    }
                }

                if (y % 2 == 0 || y == projection.Height - 1)
                {
                    frames.Add(this.BuildFrame(
                        filled,
                        title,
                        $"FILL ROW {y + 1:00}/{projection.Height:00}",
                        $"LAVA CAPACITY {volume}"));
                }
            }

            for (int i = 0; i < holdFrames; i++)
            {
                frames.Add(this.BuildFrame(
                    filled,
                    title,
                    "LAGOON COMPLETE",
                    $"LAVA CAPACITY {volume}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private LagoonProjection BuildProjection(List<(Direction Dir, long Distance)> plan, int width, int height)
        {
            long x = 0;
            long y = 0;
            long minX = 0;
            long maxX = 0;
            long minY = 0;
            long maxY = 0;

            foreach ((Direction Dir, long Distance) inst in plan)
            {
                switch (inst.Dir)
                {
                    case Direction.R: x += inst.Distance; break;
                    case Direction.L: x -= inst.Distance; break;
                    case Direction.U: y += inst.Distance; break;
                    case Direction.D: y -= inst.Distance; break;
                }

                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);
            }

            return new LagoonProjection(width, height, minX, maxX, minY, maxY);
        }

        private List<ScreenSegment> ProjectSegments(List<(Direction Dir, long Distance)> plan, LagoonProjection projection)
        {
            List<ScreenSegment> segments = [];
            long x = 0;
            long y = 0;
            ScreenPoint start = projection.Project(x, y);
            string paints = "0123456789ABCDEF";

            for (int i = 0; i < plan.Count; i++)
            {
                (Direction Dir, long Distance) inst = plan[i];

                switch (inst.Dir)
                {
                    case Direction.R: x += inst.Distance; break;
                    case Direction.L: x -= inst.Distance; break;
                    case Direction.U: y += inst.Distance; break;
                    case Direction.D: y -= inst.Distance; break;
                }

                ScreenPoint end = projection.Project(x, y);
                segments.Add(new ScreenSegment(start, end, inst.Dir, inst.Distance, paints[i % paints.Length]));
                start = end;
            }

            return segments;
        }

        private string[] BuildFrame(char[,] canvas, string title, string status, string footer)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(status);
            result.Add(footer);
            result.Add(new string('-', canvas.GetLength(1)));

            for (int y = 0; y < canvas.GetLength(0); y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < canvas.GetLength(1); x++)
                {
                    sb.Append(canvas[y, x]);
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
                this.Renderer?.RenderFrame(new Frame(PadFrame(frame, width, height)));
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

        private static char[,] CreateCanvas(int width, int height, char fill)
        {
            char[,] canvas = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    canvas[y, x] = fill;
                }
            }

            return canvas;
        }

        private static char[,] CloneCanvas(char[,] source)
        {
            char[,] result = new char[source.GetLength(0), source.GetLength(1)];

            for (int y = 0; y < source.GetLength(0); y++)
            {
                for (int x = 0; x < source.GetLength(1); x++)
                {
                    result[y, x] = source[y, x];
                }
            }

            return result;
        }

        private static void DrawLine(char[,] canvas, ScreenPoint start, ScreenPoint end, char value)
        {
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                DrawPoint(canvas, new ScreenPoint(x0, y0), value);

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = 2 * err;

                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private static void DrawPoint(char[,] canvas, ScreenPoint point, char value)
        {
            if (point.Y < 0 || point.Y >= canvas.GetLength(0) || point.X < 0 || point.X >= canvas.GetLength(1))
            {
                return;
            }

            canvas[point.Y, point.X] = value;
        }

        private static bool IsInsidePolygon(int x, int y, List<ScreenPoint> polygon)
        {
            bool inside = false;

            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                ScreenPoint a = polygon[i];
                ScreenPoint b = polygon[j];

                if (((a.Y > y) != (b.Y > y)) &&
                    x < (b.X - a.X) * (y - a.Y) / Math.Max(1.0, b.Y - a.Y) + a.X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private readonly record struct ScreenPoint(int X, int Y);

        private readonly record struct ScreenSegment(
            ScreenPoint Start,
            ScreenPoint End,
            Direction Dir,
            long Distance,
            char Paint);

        private sealed class LagoonProjection(
            int width,
            int height,
            long minX,
            long maxX,
            long minY,
            long maxY)
        {
            public int Width { get; } = width;

            public int Height { get; } = height;

            public long WorldWidth { get; } = maxX - minX + 1;

            public long WorldHeight { get; } = maxY - minY + 1;

            public ScreenPoint Project(long x, long y)
            {
                int sx = Scale(x, minX, maxX, this.Width);
                int sy = this.Height - 1 - Scale(y, minY, maxY, this.Height);

                return new ScreenPoint(sx, sy);
            }

            private static int Scale(long value, long min, long max, int size)
            {
                if (max == min)
                {
                    return size / 2;
                }

                decimal normal = (decimal)(value - min) / (max - min);
                int scaled = (int)Math.Round(normal * (size - 1));

                return Math.Clamp(scaled, 0, size - 1);
            }
        }
    }    
}
