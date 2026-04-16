namespace AdventOfCode.Puzzles._2025.Day_09___Movie_Theater
{
    using AdventOfCode.Core;

    public class MovieTheater(string[] input)
    {
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
    }
}
