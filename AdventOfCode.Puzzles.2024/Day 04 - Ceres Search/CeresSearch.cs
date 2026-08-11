namespace AdventOfCode.Puzzles._2024.Day_04___Ceres_Search
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class CeresSearch(string[] input)
    {
        public CeresSearch(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, char> Map = new(input, c => c);

        private HashSet<List<Vector<int>>> Locations = new();

        private readonly record struct SearchHit(List<Vector<int>> Points, Cardinal Direction, string Term);

        private readonly record struct XMasHit(Vector<int> Centre, List<Vector<int>> Points);

        public int XmasCount()
        {
            VectorArray<int, char> map = new(input, c => c);

            int result = 0;

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    foreach (var cardinal in Enum.GetValues<Cardinal>())
                    {
                        if (FindTerm(map, new Vector<int>(x, y), cardinal, "XMAS").Count == 4)
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        public int XmasHyphenCount()
        {
            int result = 0;

            foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
            {
                this.Search("MAS", new List<Cardinal> { Cardinal.NorthEast, Cardinal.NorthWest, Cardinal.SouthEast, Cardinal.SouthWest }, cell.Point);
                this.Search("SAM", new List<Cardinal> { Cardinal.NorthEast, Cardinal.NorthWest, Cardinal.SouthEast, Cardinal.SouthWest }, cell.Point);
            }

            this.Filter(ref result);

            return result;
        }

        public CeresSearch RenderSilver(int renderEvery = 24)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            HashSet<Vector<int>> found = [];
            List<string[]> frames = [];
            int tested = 0;
            int count = 0;

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    foreach (Cardinal cardinal in Enum.GetValues<Cardinal>())
                    {
                        List<Vector<int>> points = FindTerm(this.Map, point, cardinal, "XMAS");
                        tested++;

                        if (points.Count == 4)
                        {
                            count++;

                            foreach (Vector<int> matchPoint in points)
                            {
                                found.Add(matchPoint);
                            }

                            frames.Add(this.BuildFrame(
                                $"CERES SEARCH // XMAS {count:0000} // SCANS {tested:000000}",
                                found,
                                points,
                                point));
                        }
                        else if (tested % renderEvery == 0)
                        {
                            frames.Add(this.BuildFrame(
                                $"CERES SEARCH // XMAS {count:0000} // SCANS {tested:000000}",
                                found,
                                points,
                                point));
                        }
                    }
                }
            }

            this.AddHoldFrames(frames, found, $"CERES SEARCH COMPLETE // XMAS {count:0000}", 32);
            this.RenderPaddedFrames(frames);

            return this;
        }

        public CeresSearch RenderGold(int renderEvery = 6)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            HashSet<Vector<int>> found = [];
            List<string[]> frames = [];
            int tested = 0;
            int count = 0;

            foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
            {
                tested++;
                XMasHit? hit = this.FindXMas(cell.Point);

                if (hit != null)
                {
                    count++;

                    foreach (Vector<int> point in hit.Value.Points)
                    {
                        found.Add(point);
                    }

                    frames.Add(this.BuildFrame(
                        $"X-MAS SEARCH // FOUND {count:0000} // CENTRE {cell.Point.X:000},{cell.Point.Y:000}",
                        found,
                        hit.Value.Points,
                        cell.Point));
                }
                else if (tested % renderEvery == 0)
                {
                    frames.Add(this.BuildFrame(
                        $"X-MAS SEARCH // FOUND {count:0000} // CELLS {tested:00000}",
                        found,
                        [],
                        cell.Point));
                }
            }

            this.AddHoldFrames(frames, found, $"X-MAS SEARCH COMPLETE // FOUND {count:0000}", 32);
            this.RenderPaddedFrames(frames);

            return this;
        }

        private void Search(string term, List<Cardinal> directions, Vector<int> point)
        {
            foreach (Cardinal direction in directions)
            {
                List<Vector<int>> points = FindTerm(this.Map, point, direction, term);

                if (points.Count == term.Length)
                {
                    this.Locations.Add(points);
                }
            }
        }

        private void Filter(ref int result)
        {
            Dictionary<string, Vector<int>> processed = new();
            List<Vector<int>> tree = new();

            foreach (var locationA in this.Locations)
            {
                if (processed.ContainsKey(locationA[1].ToKey2D()))
                {
                    continue;
                }

                foreach (var locationB in this.Locations)
                {
                    if (locationA != locationB
                        && locationA[1] == locationB[1])
                    {
                        tree.Clear();
                        tree.AddRange(locationA);
                        tree.AddRange(locationB);

                        if (new[]
                        {
                            locationA[1] + Vector<int>.NorthWest,
                            locationA[1] + Vector<int>.NorthEast,
                            locationA[1] + Vector<int>.SouthWest,
                            locationA[1] + Vector<int>.SouthEast
                        }.All(tree.Contains))
                        {
                            processed.Add(locationA[1].ToKey2D(), locationA[1]);
                            result++;
                        }

                        break;
                    }
                }
            }
        }

        private XMasHit? FindXMas(Vector<int> centre)
        {
            if (this.Map[centre] != 'A')
            {
                return null;
            }

            Vector<int> northWest = centre + Vector<int>.NorthWest;
            Vector<int> northEast = centre + Vector<int>.NorthEast;
            Vector<int> southWest = centre + Vector<int>.SouthWest;
            Vector<int> southEast = centre + Vector<int>.SouthEast;

            if (!this.Map.IsVectorInRange(northWest)
                || !this.Map.IsVectorInRange(northEast)
                || !this.Map.IsVectorInRange(southWest)
                || !this.Map.IsVectorInRange(southEast))
            {
                return null;
            }

            string diagonalA = $"{this.Map[northWest]}{this.Map[centre]}{this.Map[southEast]}";
            string diagonalB = $"{this.Map[northEast]}{this.Map[centre]}{this.Map[southWest]}";

            if ((diagonalA == "MAS" || diagonalA == "SAM")
                && (diagonalB == "MAS" || diagonalB == "SAM"))
            {
                return new(centre, [northWest, northEast, centre, southWest, southEast]);
            }

            return null;
        }

        private static List<Vector<int>> FindTerm(VectorArray<int, char> map, Vector<int> point, Cardinal direction, string term)
        {
            List<Vector<int>> points = new();

            if (map[point] != term[0])
            {
                return points;
            }

            points.Add(point);

            for (int i = 1; i <= term.Length - 1; i++)
            {
                var adjacent = map.AdjacentInterCardinal(point);

                if (!adjacent.Any(x => x.Direction == direction && x.Value == term[i]))
                {
                    return points;
                }

                point = adjacent.First(x => x.Direction == direction).Point;
                points.Add(point);
            }

            return points;
        }

        private string[] BuildFrame(
            string title,
            HashSet<Vector<int>> found,
            List<Vector<int>> active,
            Vector<int> cursor)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(new string('-', Math.Max(title.Length, this.Map.Width)));

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Map[y, x];

                    if (point == cursor)
                    {
                        sb.Append('@');
                    }
                    else if (active.Contains(point))
                    {
                        sb.Append(char.ToLowerInvariant(value));
                    }
                    else if (found.Contains(point))
                    {
                        sb.Append(char.ToLowerInvariant(value));
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private void AddHoldFrames(
            List<string[]> frames,
            HashSet<Vector<int>> found,
            string title,
            int count)
        {
            string[] frame = this.BuildFrame(title, found, [], new(-1, -1));

            for (int i = 0; i < count; i++)
            {
                frames.Add(frame);
            }
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
