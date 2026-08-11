namespace AdventOfCode.Puzzles._2022.Day_12___Hill_Climbing_Algorithm
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class HillClimbingAlgorithm
    {
        private const int CameraWidth = 46;
        private const int CameraHeight = 24;
        private const int RenderGutter = 4;

        public HillClimbingAlgorithm(string[] input, bool isStartOnly = true)
        {
            this.Positions = [];
            this.Finish = new(0, 0);
            this.Map = this.Parse(input, isStartOnly);
        }

        public HillClimbingAlgorithm(string[] input, IFrameRenderer renderer, bool isStartOnly = true)
            : this(input, isStartOnly)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, int> Map { get; }

        private List<Vector<int>> Positions { get; }

        private Vector<int> Finish { get; set; }

        public int Fewest()
            => this.FindPath().Count;

        public HillClimbingAlgorithm RenderSilver(int renderEvery = 3)
        {
            return this.RenderPath(
                "HILL CLIMBING ALGORITHM",
                renderEvery);
        }

        public HillClimbingAlgorithm RenderGold(int renderEvery = 3)
        {
            return this.RenderPath(
                "SCENIC HIKING TRAIL",
                renderEvery);
        }

        private HillClimbingAlgorithm RenderPath(string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<int>> path = this.FindPath();
            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                Vector<int> point = path[i];

                trail.Add(point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    this.Renderer.RenderFrame(
                        new Frame(
                            this.BuildFrame(
                                point,
                                trail,
                                $"{title} // STEPS {i + 1:0000}")));
                }
            }

            return this;
        }

        private List<Vector<int>> FindPath()
        {
            Queue<Vector<int>> queue = new();
            HashSet<Vector<int>> visited = [];

            Dictionary<Vector<int>, Vector<int>> cameFrom = [];

            queue.Enqueue(this.Finish);
            visited.Add(this.Finish);

            while (queue.Count > 0)
            {
                Vector<int> current = queue.Dequeue();

                if (this.Positions.Contains(current))
                {
                    return ReconstructPath(cameFrom, current);
                }

                int elevation = this.Map.GetValue(current);

                foreach (VectorCell<int, int> adjacent in this.Map.AdjacentCardinal(current))
                {
                    if (visited.Contains(adjacent.Point))
                    {
                        continue;
                    }

                    if (adjacent.Value < elevation - 1)
                    {
                        continue;
                    }

                    visited.Add(adjacent.Point);

                    cameFrom[adjacent.Point] = current;

                    queue.Enqueue(adjacent.Point);
                }
            }

            return [];
        }

        private static List<Vector<int>> ReconstructPath(
            Dictionary<Vector<int>, Vector<int>> cameFrom,
            Vector<int> start)
        {
            List<Vector<int>> path = [start];

            Vector<int> current = start;

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        private VectorArray<int, int> Parse(string[] input, bool isStartOnly)
        {
            return new(input, (c, x, y) =>
            {
                char value = c;

                if (value == 'S')
                {
                    this.Positions.Add(new(x, y));
                    value = 'a';
                }

                if (value == 'E')
                {
                    this.Finish = new(x, y);
                    value = 'z';
                }

                if (!isStartOnly && value == 'a')
                {
                    Vector<int> position = new(x, y);

                    if (!this.Positions.Contains(position))
                    {
                        this.Positions.Add(position);
                    }
                }

                return value;
            });
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> trail,
            string title)
        {
            List<string> result = [];

            int viewWidth = Math.Min(CameraWidth, this.Map.Width);
            int viewHeight = Math.Min(CameraHeight, this.Map.Height);

            int startX = current.X - (viewWidth / 2);
            int startY = current.Y - (viewHeight / 2);

            startX = Math.Clamp(startX, 0, this.Map.Width - viewWidth);
            startY = Math.Clamp(startY, 0, this.Map.Height - viewHeight);

            int endX = startX + viewWidth;
            int endY = startY + viewHeight;

            int renderWidth = Math.Max(title.Length, viewWidth) + RenderGutter;

            result.Add(title.PadRight(renderWidth, ' '));
            result.Add(new string(' ', renderWidth));

            for (int y = startY; y < endY; y++)
            {
                StringBuilder sb = new();

                for (int x = startX; x < endX; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = (char)this.Map[y, x];

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (point == this.Finish)
                    {
                        sb.Append('E');
                    }
                    else if (this.Positions.Contains(point))
                    {
                        sb.Append('S');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }

                result.Add(sb.ToString().PadRight(renderWidth, ' '));
            }

            return [.. result];
        }
    }
}