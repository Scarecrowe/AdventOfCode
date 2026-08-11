namespace AdventOfCode.Puzzles._2018.Day_06___Chronal_Coordinates
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class ChronalCoordinates
    {
        private const int SafeViewportWidth = 120;
        private const int SafeViewportHeight = 54;

        public ChronalCoordinates(string[] input)
        {
            this.Coordinates = new();
            this.Locations = new();
            this.Map = new();

            this.Parse(input);
        }

        public ChronalCoordinates(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public Dictionary<char, Vector<int>> Coordinates { get; private set; }

        public List<Vector<int>> Locations { get; private set; }

        public VectorDictionary<int, char> Map { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Dangerous()
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    Dictionary<char, int> distances = this.DistanceToCoordinates(point);
                    int min = distances.Min(c => c.Value);

                    if (distances.Count(c => c.Value == min) == 1)
                    {
                        this.Map.Add(point, distances.FirstOrDefault(c => c.Value == min).Key);
                    }
                    else
                    {
                        this.Map.Add(point, '.');
                    }
                }
            }

            int result = 0;

            foreach (KeyValuePair<char, Vector<int>> coordinate in this.Coordinates)
            {
                if (!this.Map.IsEdge(coordinate.Key))
                {
                    result = Math.Max(result, this.Map.Count(x => x.Value == coordinate.Key));
                }
            }

            return result;
        }

        public int Safe(int distance)
        {
            int max = distance / 25;
            int min = max * -1;
            int count = 0;

            for (int y = min; y < max; y++)
            {
                for (int x = min; x < max; x++)
                {
                    if (this.Locations.Sum(point => new Vector<int>(x, y).Distance(point)) < distance)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public ChronalCoordinates RenderSilver(int renderEvery = 100)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            Dictionary<char, int> areas = new();
            HashSet<char> infinite = [];
            HashSet<Vector<int>> coordinatePoints = this.Coordinates.Values.ToHashSet();

            int processed = 0;

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    char value = '.';
                    int bestDistance = int.MaxValue;
                    bool tied = false;

                    foreach (KeyValuePair<char, Vector<int>> coordinate in this.Coordinates)
                    {
                        int distance = (int)point.Distance(coordinate.Value);

                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            value = coordinate.Key;
                            tied = false;
                        }
                        else if (distance == bestDistance)
                        {
                            tied = true;
                        }
                    }

                    if (tied)
                    {
                        value = '.';
                    }

                    this.Map[point] = value;

                    if (value != '.')
                    {
                        areas.TryAdd(value, 0);
                        areas[value]++;

                        if (x == 0 || y == 0 || x == this.Width - 1 || y == this.Height - 1)
                        {
                            infinite.Add(value);
                        }
                    }

                    processed++;

                    if (processed == 1 || processed % renderEvery == 0)
                    {
                        this.Renderer.RenderFrame(
                            new Frame(this.BuildDangerFrame(point, areas, infinite, coordinatePoints, false)));
                    }
                }
            }

            int largest = areas
                .Where(area => !infinite.Contains(area.Key))
                .Select(area => area.Value)
                .DefaultIfEmpty(0)
                .Max();

            Vector<int> last = new(this.Width - 1, this.Height - 1);

            return this;
        }

        public ChronalCoordinates RenderGold(int distance = 10000, int renderEvery = 2500)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int max = distance / 25;
            int min = max * -1;

            int count = 0;
            int processed = 0;

            HashSet<Vector<int>> safe = [];
            HashSet<Vector<int>> coordinatePoints = this.Locations.ToHashSet();

            Vector<int> first = new(min, min);

            this.Renderer.RenderFrame(
                new Frame(this.BuildSafeFrame(first, safe, coordinatePoints, distance, count, false)));

            for (int y = min; y < max; y++)
            {
                for (int x = min; x < max; x++)
                {
                    Vector<int> point = new(x, y);

                    int totalDistance = 0;

                    foreach (Vector<int> location in this.Locations)
                    {
                        totalDistance += Math.Abs(point.X - location.X) + Math.Abs(point.Y - location.Y);

                        if (totalDistance >= distance)
                        {
                            break;
                        }
                    }

                    if (totalDistance < distance)
                    {
                        safe.Add(point);
                        count++;
                    }

                    processed++;

                    if (processed % renderEvery == 0)
                    {
                        this.Renderer.RenderFrame(
                            new Frame(this.BuildSafeFrame(point, safe, coordinatePoints, distance, count, false)));
                    }
                }
            }

            Vector<int> last = new(max - 1, max - 1);

            return this;
        }

        private void Parse(string[] input)
        {
            this.Map = new();
            this.Coordinates = new();
            this.Locations = new();

            int letter = 'A';

            foreach (string line in input)
            {
                int[] coordinates = line.Split(", ").ToInt();

                this.Coordinates.Add((char)letter, new(coordinates));

                this.Width = Math.Max(this.Width, coordinates[0]);
                this.Height = Math.Max(this.Height, coordinates[1]);

                this.Map.Add(new(coordinates), (char)letter);
                this.Locations.Add(new(coordinates));

                letter++;
            }
        }

        private Dictionary<char, int> DistanceToCoordinates(Vector<int> point)
        {
            Dictionary<char, int> result = new();

            foreach (KeyValuePair<char, Vector<int>> coordinate in this.Coordinates)
            {
                result.Add(coordinate.Key, (int)point.Distance(coordinate.Value));
            }

            return result;
        }

        private string[] BuildDangerFrame(
            Vector<int> current,
            Dictionary<char, int> areas,
            HashSet<char> infinite,
            HashSet<Vector<int>> coordinatePoints,
            bool complete,
            string? title = null)
        {
            List<string> result = [];

            int largest = areas
                .Where(area => !infinite.Contains(area.Key))
                .Select(area => area.Value)
                .DefaultIfEmpty(0)
                .Max();

            result.Add(title ?? (complete
                ? $"CHRONAL COORDINATES // LARGEST FINITE AREA {largest:00000}"
                : $"CLAIMING CHRONAL AREAS // X {current.X:000} Y {current.Y:000} // BEST {largest:00000}"));

            result.Add(string.Empty);

            for (int y = 0; y < this.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    char value = this.Map.TryGetValue(point, out char mapped)
                        ? mapped
                        : ' ';

                    if (point == current && !complete)
                    {
                        sb.Append('@');
                    }
                    else if (coordinatePoints.Contains(point))
                    {
                        sb.Append('O');
                    }
                    else if (value == '.')
                    {
                        sb.Append('.');
                    }
                    else if (value == ' ')
                    {
                        sb.Append(' ');
                    }
                    else if (infinite.Contains(value))
                    {
                        sb.Append('~');
                    }
                    else
                    {
                        sb.Append('#');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildSafeFrame(
            Vector<int> current,
            HashSet<Vector<int>> safe,
            HashSet<Vector<int>> coordinatePoints,
            int distance,
            int count,
            bool complete,
            string? title = null)
        {
            const int viewportWidth = 80;
            const int viewportHeight = 32;

            List<string> result = [];

            int startX = current.X - (viewportWidth / 2);
            int startY = current.Y - (viewportHeight / 2);

            result.Add(title ?? (complete
                ? $"SAFE REGION // DISTANCE < {distance} // SIZE {count:00000}"
                : $"SCANNING SAFE REGION // X {current.X:000} Y {current.Y:000} // SAFE {count:00000}"));

            result.Add($"VIEWPORT X {startX:0000}..{startX + viewportWidth - 1:0000} // Y {startY:0000}..{startY + viewportHeight - 1:0000}");
            result.Add(string.Empty);

            for (int y = startY; y < startY + viewportHeight; y++)
            {
                char[] row = new char[viewportWidth];

                for (int i = 0; i < viewportWidth; i++)
                {
                    row[i] = '.';
                }

                for (int x = startX; x < startX + viewportWidth; x++)
                {
                    Vector<int> point = new(x, y);
                    int index = x - startX;

                    if (point == current && !complete)
                    {
                        row[index] = '@';
                    }
                    else if (coordinatePoints.Contains(point))
                    {
                        row[index] = 'O';
                    }
                    else if (safe.Contains(point))
                    {
                        row[index] = '#';
                    }
                }

                result.Add(new string(row));
            }

            return [.. result];
        }

        private static (int X, int Y) GetViewportStart(Vector<int> current)
            => (
                current.X - (SafeViewportWidth / 2),
                current.Y - (SafeViewportHeight / 2));

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