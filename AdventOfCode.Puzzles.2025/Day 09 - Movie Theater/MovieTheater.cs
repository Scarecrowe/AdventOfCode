namespace AdventOfCode.Puzzles._2025.Day_09___Movie_Theater
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class MovieTheater(string[] input)
    {
        public MovieTheater(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private const int MaxRenderWidth = 140;
        private const int MaxRenderHeight = 48;
        private const int HoldFrames = 24;

        public long AreaOutside()
        {
            HashSet<Vector<long>> points = ParseHashSet(input);

            (long minX, _, long minY, _) = GetBounds(points);
            HashSet<Vector<long>> shiftedPoints = ShiftPoints(points, minX, minY);

            (Vector<long> bestA, Vector<long> bestB, _) = FindFarthestPair(shiftedPoints);

            return CalculateBoundingArea(bestA, bestB);
        }

        public long AreaInside()
        {
            (int X, int Y)[] points = this.ParseTupleArray(input);

            Dictionary<int, List<int>> groupX = GroupPointsByX(points);
            Dictionary<int, List<int>> groupY = GroupPointsByY(points);
            List<((int X, int Y) a, (int X, int Y) b)> horizontalSegments = BuildVerticalSpans(groupX);
            List<((int X, int Y) a, (int X, int Y) b)> verticalSegments = BuildHorizontalSpans(groupY);

            return FindMaxArea(points, horizontalSegments, verticalSegments);
        }

        public MovieTheater RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            (int X, int Y)[] points = this.ParseTupleArray(input);
            List<FrameState> frames = [];

            long bestArea = 0;
            (int X, int Y) bestA = default;
            (int X, int Y) bestB = default;
            long tested = 0;
            long total = ((long)points.Length * (points.Length - 1)) / 2;

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    tested++;

                    long area = CalculateArea(points[i], points[j]);

                    if (area > bestArea)
                    {
                        bestArea = area;
                        bestA = points[i];
                        bestB = points[j];

                        if (frames.Count % Math.Max(1, renderEvery) == 0)
                        {
                            frames.Add(new(
                                "MOVIE THEATER // PART 1 // LARGEST OUTSIDE RECTANGLE",
                                bestA,
                                bestB,
                                bestArea,
                                tested,
                                total,
                                false));
                        }
                    }
                }
            }

            frames.Add(new(
                "MOVIE THEATER // PART 1 COMPLETE",
                bestA,
                bestB,
                bestArea,
                tested,
                total,
                false));

            this.RenderStates(points, frames);

            return this;
        }

        public MovieTheater RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            (int X, int Y)[] points = this.ParseTupleArray(input);

            Dictionary<int, List<int>> groupX = GroupPointsByX(points);
            Dictionary<int, List<int>> groupY = GroupPointsByY(points);
            List<((int X, int Y) a, (int X, int Y) b)> horizontalSegments = BuildVerticalSpans(groupX);
            List<((int X, int Y) a, (int X, int Y) b)> verticalSegments = BuildHorizontalSpans(groupY);

            List<FrameState> frames = [];

            long bestArea = 0;
            (int X, int Y) bestA = default;
            (int X, int Y) bestB = default;
            long tested = 0;
            long total = ((long)points.Length * (points.Length - 1)) / 2;

            for (int i = 0; i < points.Length; i++)
            {
                (int X, int Y) a = points[i];

                for (int j = 0; j < i; j++)
                {
                    tested++;

                    (int X, int Y) b = points[j];

                    if (!RectangleIsClear(a, b, horizontalSegments, verticalSegments))
                    {
                        continue;
                    }

                    long area = CalculateArea(a, b);

                    if (area > bestArea)
                    {
                        bestArea = area;
                        bestA = a;
                        bestB = b;

                        if (frames.Count % Math.Max(1, renderEvery) == 0)
                        {
                            frames.Add(new(
                                "MOVIE THEATER // PART 2 // LARGEST INSIDE RECTANGLE",
                                bestA,
                                bestB,
                                bestArea,
                                tested,
                                total,
                                true));
                        }
                    }
                }
            }

            frames.Add(new(
                "MOVIE THEATER // PART 2 COMPLETE",
                bestA,
                bestB,
                bestArea,
                tested,
                total,
                true));

            this.RenderStates(points, frames);

            return this;
        }

        private void RenderStates((int X, int Y)[] points, List<FrameState> states)
        {
            List<string[]> frames = [];

            foreach (FrameState state in states)
            {
                frames.Add(this.BuildFrame(points, state));
            }

            for (int i = 0; i < HoldFrames; i++)
            {
                frames.Add(this.BuildFrame(points, states.Last()));
            }

            this.RenderPaddedFrames(frames);
        }

        private string[] BuildFrame((int X, int Y)[] points, FrameState state)
        {
            CoordinateMap map = new(points);
            RenderWindow window = RenderWindow.Create(map, state.A, state.B, MaxRenderWidth, MaxRenderHeight);
            char[,] canvas = CreateCanvas(window.Width, window.Height, ' ');

            DrawLoop(points, map, window, canvas, state.ShowInside);
            DrawRectangle(state.A, state.B, map, window, canvas);
            DrawRedPoints(points, map, window, canvas);
            DrawPoint(state.A, map, window, canvas, 'A');
            DrawPoint(state.B, map, window, canvas, 'B');

            List<string> result = [];
            result.Add(state.Title);
            result.Add($"AREA {state.Area} // TESTED {state.Tested}/{state.Total}");
            result.Add($"A {state.A.X},{state.A.Y} // B {state.B.X},{state.B.Y}");

            if (window.Cropped)
            {
                result.Add($"COMPRESSED VIEWPORT {window.X0},{window.Y0} -> {window.X1},{window.Y1}");
            }
            else
            {
                result.Add("COMPRESSED FULL VIEW");
            }

            result.Add(string.Empty);

            for (int y = 0; y < window.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < window.Width; x++)
                {
                    sb.Append(canvas[y, x]);
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static void DrawLoop((int X, int Y)[] points, CoordinateMap map, RenderWindow window, char[,] canvas, bool fillInside)
        {
            if (fillInside)
            {
                DrawCompressedInterior(points, map, window, canvas);
            }

            for (int i = 0; i < points.Length; i++)
            {
                (int X, int Y) a = points[i];
                (int X, int Y) b = points[(i + 1) % points.Length];

                int ax = map.X[a.X];
                int ay = map.Y[a.Y];
                int bx = map.X[b.X];
                int by = map.Y[b.Y];

                if (ax == bx)
                {
                    DrawLine(ax, Math.Min(ay, by), ax, Math.Max(ay, by), window, canvas, '|');
                }
                else if (ay == by)
                {
                    DrawLine(Math.Min(ax, bx), ay, Math.Max(ax, bx), ay, window, canvas, '-');
                }
            }
        }

        private static void DrawCompressedInterior((int X, int Y)[] points, CoordinateMap map, RenderWindow window, char[,] canvas)
        {
            for (int cy = window.Y0; cy <= window.Y1; cy++)
            {
                long y = map.YValues[cy];

                for (int cx = window.X0; cx <= window.X1; cx++)
                {
                    long x = map.XValues[cx];

                    if (PointInsidePolygon(x, y, points))
                    {
                        Set(canvas, window, cx, cy, '.');
                    }
                }
            }
        }

        private static bool PointInsidePolygon(long x, long y, (int X, int Y)[] points)
        {
            bool inside = false;

            for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
            {
                long xi = points[i].X;
                long yi = points[i].Y;
                long xj = points[j].X;
                long yj = points[j].Y;

                bool intersects = yi > y != yj > y
                    && x < ((xj - xi) * (y - yi) / (double)(yj - yi)) + xi;

                if (intersects)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private static void DrawRectangle((int X, int Y) a, (int X, int Y) b, CoordinateMap map, RenderWindow window, char[,] canvas)
        {
            int x0 = Math.Min(map.X[a.X], map.X[b.X]);
            int x1 = Math.Max(map.X[a.X], map.X[b.X]);
            int y0 = Math.Min(map.Y[a.Y], map.Y[b.Y]);
            int y1 = Math.Max(map.Y[a.Y], map.Y[b.Y]);

            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    Set(canvas, window, x, y, 'O');
                }
            }
        }

        private static void DrawRedPoints((int X, int Y)[] points, CoordinateMap map, RenderWindow window, char[,] canvas)
        {
            foreach ((int X, int Y) point in points)
            {
                DrawPoint(point, map, window, canvas, '#');
            }
        }

        private static void DrawPoint((int X, int Y) point, CoordinateMap map, RenderWindow window, char[,] canvas, char c)
        {
            Set(canvas, window, map.X[point.X], map.Y[point.Y], c);
        }

        private static void DrawLine(int x0, int y0, int x1, int y1, RenderWindow window, char[,] canvas, char c)
        {
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    Set(canvas, window, x, y, c);
                }
            }
        }

        private static void Set(char[,] canvas, RenderWindow window, int x, int y, char c)
        {
            if (x < window.X0 || x > window.X1 || y < window.Y0 || y > window.Y1)
            {
                return;
            }

            canvas[y - window.Y0, x - window.X0] = c;
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

        private HashSet<Vector<long>> ParseHashSet(string[] input)
        {
            HashSet<Vector<long>> points = new();

            foreach (string line in input)
            {
                points.Add(ParseOutsidePoint(line));
            }

            return points;
        }

        private (int X, int Y)[] ParseTupleArray(string[] input)
        {
            (int X, int Y)[] result = new (int, int)[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ParseInsidePoint(input[i]);
            }

            return result;
        }

        private Vector<long> ParseOutsidePoint(string line)
        {
            long[] coords = ParseLongCoordinates(line);

            return new Vector<long>(coords[0], coords[1], 0);
        }

        private long[] ParseLongCoordinates(string line)
        {
            return line.Split(',').Select(long.Parse).ToArray();
        }

        private (int X, int Y) ParseInsidePoint(string line)
        {
            string[] tokens = line.Split(',');
            return (int.Parse(tokens[0]), int.Parse(tokens[1]));
        }

        private HashSet<Vector<long>> ShiftPoints(HashSet<Vector<long>> points, long minX, long minY)
        {
            HashSet<Vector<long>> result = new();

            foreach (Vector<long> point in points)
            {
                result.Add(ShiftPoint(point, minX, minY));
            }

            return result;
        }

        private Vector<long> ShiftPoint(Vector<long> point, long minX, long minY)
        {
            return new Vector<long>(point.X - minX, point.Y - minY, 0);
        }

        private (long MinX, long MaxX, long MinY, long MaxY) GetBounds(HashSet<Vector<long>> points)
        {
            long minX = points.Min(p => p.X);
            long maxX = points.Max(p => p.X);
            long minY = points.Min(p => p.Y);
            long maxY = points.Max(p => p.Y);

            return (minX, maxX, minY, maxY);
        }

        private (Vector<long> A, Vector<long> B, long Distance) FindFarthestPair(HashSet<Vector<long>> points)
        {
            List<Vector<long>> list = [.. points];

            Vector<long> bestA = default!;
            Vector<long> bestB = default!;
            long bestDist = long.MinValue;

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    Vector<long> a = list[i];
                    Vector<long> b = list[j];

                    long dist = GetManhattanDistance(a, b);

                    if (dist > bestDist)
                    {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            return (bestA, bestB, bestDist);
        }

        private long GetManhattanDistance(Vector<long> a, Vector<long> b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private long CalculateBoundingArea(Vector<long> a, Vector<long> b)
        {
            long width = Math.Abs(a.X - b.X) + 1;
            long height = Math.Abs(a.Y - b.Y) + 1;

            return width * height;
        }

        private static Dictionary<int, List<int>> GroupPointsByX((int X, int Y)[] points)
        {
            Dictionary<int, List<int>> groupX = [];

            foreach ((int x, int y) in points)
            {
                if (!groupX.ContainsKey(x))
                {
                    groupX[x] = [];
                }

                groupX[x].Add(y);
            }

            return groupX;
        }

        private static Dictionary<int, List<int>> GroupPointsByY((int X, int Y)[] points)
        {
            Dictionary<int, List<int>> groupY = [];

            foreach ((int x, int y) in points)
            {
                if (!groupY.ContainsKey(y))
                {
                    groupY[y] = [];
                }

                groupY[y].Add(x);
            }

            return groupY;
        }

        private static List<((int X, int Y) a, (int X, int Y) b)> BuildVerticalSpans(Dictionary<int, List<int>> groupX)
        {
            List<((int X, int Y) a, (int X, int Y) b)> spans = [];

            foreach (KeyValuePair<int, List<int>> pair in groupX)
            {
                List<int> sorted = pair.Value.OrderBy(y => y).ToList();

                for (int i = 0; i < sorted.Count / 2; i++)
                {
                    (int Key, int) a = (pair.Key, sorted[2 * i]);
                    (int Key, int) b = (pair.Key, sorted[2 * i + 1]);

                    spans.Add((a, b));
                }
            }

            return spans;
        }

        private static List<((int X, int Y) a, (int X, int Y) b)> BuildHorizontalSpans(Dictionary<int, List<int>> groupY)
        {
            List<((int X, int Y) a, (int X, int Y) b)> spans = [];

            foreach (KeyValuePair<int, List<int>> pair in groupY)
            {
                List<int> sorted = pair.Value.OrderBy(x => x).ToList();

                for (int i = 0; i < sorted.Count / 2; i++)
                {
                    (int, int Key) a = (sorted[2 * i], pair.Key);
                    (int, int Key) b = (sorted[2 * i + 1], pair.Key);

                    spans.Add((a, b));
                }
            }

            return spans;
        }

        private static long FindMaxArea(
            (int X, int Y)[] points,
            List<((int X, int Y) a, (int X, int Y) b)> horizontalSegments,
            List<((int X, int Y) a, (int X, int Y) b)> verticalSegments)
        {
            long maxAreaInside = 0;

            for (int i = 0; i < points.Length; i++)
            {
                (int X, int Y) a = points[i];

                for (int j = 0; j < i; j++)
                {
                    (int X, int Y) b = points[j];

                    if (!RectangleIsClear(a, b, horizontalSegments, verticalSegments))
                    {
                        continue;
                    }

                    long area = CalculateArea(a, b);

                    if (area > maxAreaInside)
                    {
                        maxAreaInside = area;
                    }
                }
            }

            return maxAreaInside;
        }

        private static bool RectangleIsClear(
            (int X, int Y) a,
            (int X, int Y) b,
            List<((int X, int Y) a, (int X, int Y) b)> horizontalSegments,
            List<((int X, int Y) a, (int X, int Y) b)> verticalSegments)
        {
            int minX = Math.Min(a.X, b.X);
            int maxX = Math.Max(a.X, b.X);
            int minY = Math.Min(a.Y, b.Y);
            int maxY = Math.Max(a.Y, b.Y);

            foreach (((int X, int Y) h0, (int X, int Y) h1) in horizontalSegments)
            {
                int hx = h0.X;
                int hy0 = Math.Min(h0.Y, h1.Y);
                int hy1 = Math.Max(h0.Y, h1.Y);

                if (hx > minX && hx < maxX)
                {
                    if (!(hy1 <= minY || hy0 >= maxY))
                    {
                        return false;
                    }
                }
            }

            foreach (((int X, int Y) v0, (int X, int Y) v1) in verticalSegments)
            {
                int vy = v0.Y;
                int vx0 = Math.Min(v0.X, v1.X);
                int vx1 = Math.Max(v0.X, v1.X);

                if (vy > minY && vy < maxY)
                {
                    if (!(vx1 <= minX || vx0 >= maxX))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static long CalculateArea((int X, int Y) a, (int X, int Y) b)
        {
            return (long)(Math.Abs(a.X - b.X) + 1) * (Math.Abs(a.Y - b.Y) + 1);
        }

        private sealed record FrameState(
            string Title,
            (int X, int Y) A,
            (int X, int Y) B,
            long Area,
            long Tested,
            long Total,
            bool ShowInside);

        private sealed class CoordinateMap
        {
            public CoordinateMap((int X, int Y)[] points)
            {
                this.XValues = points.Select(p => (long)p.X).Distinct().OrderBy(x => x).ToArray();
                this.YValues = points.Select(p => (long)p.Y).Distinct().OrderBy(y => y).ToArray();
                this.X = this.XValues.Select((value, index) => ((int)value, index)).ToDictionary(x => x.Item1, x => x.index);
                this.Y = this.YValues.Select((value, index) => ((int)value, index)).ToDictionary(y => y.Item1, y => y.index);
            }

            public long[] XValues { get; }

            public long[] YValues { get; }

            public Dictionary<int, int> X { get; }

            public Dictionary<int, int> Y { get; }
        }

        private sealed record RenderWindow(int X0, int X1, int Y0, int Y1)
        {
            public int Width => this.X1 - this.X0 + 1;

            public int Height => this.Y1 - this.Y0 + 1;

            public bool Cropped => this.X0 != 0 || this.Y0 != 0;

            public static RenderWindow Create(CoordinateMap map, (int X, int Y) a, (int X, int Y) b, int maxWidth, int maxHeight)
            {
                int fullWidth = map.XValues.Length;
                int fullHeight = map.YValues.Length;

                int rectX0 = Math.Min(map.X[a.X], map.X[b.X]);
                int rectX1 = Math.Max(map.X[a.X], map.X[b.X]);
                int rectY0 = Math.Min(map.Y[a.Y], map.Y[b.Y]);
                int rectY1 = Math.Max(map.Y[a.Y], map.Y[b.Y]);

                int centerX = (rectX0 + rectX1) / 2;
                int centerY = (rectY0 + rectY1) / 2;

                int width = Math.Min(fullWidth, maxWidth);
                int height = Math.Min(fullHeight, maxHeight);

                int x0 = Clamp(centerX - width / 2, 0, Math.Max(0, fullWidth - width));
                int y0 = Clamp(centerY - height / 2, 0, Math.Max(0, fullHeight - height));

                return new(x0, x0 + width - 1, y0, y0 + height - 1);
            }

            private static int Clamp(int value, int min, int max)
            {
                return Math.Max(min, Math.Min(max, value));
            }
        }
    }
}
