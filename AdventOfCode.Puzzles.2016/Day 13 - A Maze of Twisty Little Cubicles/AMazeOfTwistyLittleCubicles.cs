namespace AdventOfCode.Puzzles._2016.Day_13___A_Maze_of_Twisty_Little_Cubicles
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class AMazeOfTwistyLittleCubicles
    {
        public AMazeOfTwistyLittleCubicles(
            string[] input,
            int targetX,
            int targetY)
        {
            int favorite = input[0].ToInt();
            this.Target = new(targetX, targetY);
            this.Paths = new();
            this.Visited = new();
            this.Map = CreateMap(favorite, this.Target + 10);
        }

        public AMazeOfTwistyLittleCubicles(
            string[] input,
            int targetX,
            int targetY,
            IFrameRenderer renderer)
            : this(input, targetX, targetY)
        {
            this.Renderer = renderer;
        }

        public VectorDictionary<int, int> Map { get; }

        public Vector<int> Target { get; }

        public IFrameRenderer? Renderer { get; }

        private List<long> Paths { get; set; }

        private HashSet<Vector<int>> Visited { get; set; }

        private readonly record struct PathNode(Vector<int> Point, int Steps);

        public long ShortestPath() => this.Paths.Min();

        public long UniqueLocations() => this.Visited.Count;

        public AMazeOfTwistyLittleCubicles FindAllPaths()
        {
            this.Paths = new();
            this.Visited = new();
            this.FindPath(new(1, 1), 0, new() { new(1, 1) });

            return this;
        }

        public AMazeOfTwistyLittleCubicles RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<int>> path = this.GetShortestPath();
            HashSet<Vector<int>> trail = [];
            List<string[]> frames = [];

            for (int i = 0; i < path.Count; i++)
            {
                Vector<int> point = path[i];
                trail.Add(point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        current: point,
                        trail: trail,
                        searched: [],
                        title: $"A MAZE OF TWISTY LITTLE CUBICLES // TARGET {this.Target.X},{this.Target.Y} // STEPS {i:000}"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    current: this.Target,
                    trail: trail,
                    searched: [],
                    title: $"TARGET REACHED // FEWEST STEPS {path.Count - 1:000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public AMazeOfTwistyLittleCubicles RenderGold(int renderEvery = 2)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PathNode> search = this.GetReachableSearch(50);
            HashSet<Vector<int>> reached = [];
            List<string[]> frames = [];

            for (int i = 0; i < search.Count; i++)
            {
                PathNode node = search[i];
                reached.Add(node.Point);

                if (i % renderEvery == 0 || i == search.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        current: node.Point,
                        trail: [],
                        searched: reached,
                        title: $"REACHABLE LOCATIONS // LIMIT 50 STEPS // COUNT {reached.Count:000}"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                PathNode last = search.Last();

                frames.Add(this.BuildFrame(
                    current: last.Point,
                    trail: [],
                    searched: reached,
                    title: $"TOTAL LOCATIONS REACHED IN 50 STEPS // {reached.Count:000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private static bool IsOpenSpace(Vector<int> point, int favorite)
            => (((point.X * point.X) + (3 * point.X) + (2 * point.X * point.Y) + point.Y + (point.Y * point.Y)) + favorite)
                .ToBinary()
                .Count(x => x == '1') % 2 == 0;

        private static VectorDictionary<int, int> CreateMap(int favorite, Vector<int> target)
        {
            VectorDictionary<int, int> map = new();

            Vector<int>.AxisEnumerator(target.X, target.Y).ForEach(point =>
            {
                if (IsOpenSpace(point, favorite))
                {
                    map.Add(point, 1);
                }
            });

            return map;
        }

        private void FindPath(Vector<int> point, int steps, HashSet<Vector<int>> visited)
        {
            visited.Add(point);

            if (point == this.Target)
            {
                this.Paths.Add(steps);
                return;
            }

            if (steps <= 50)
            {
                foreach (Vector<int> visit in visited)
                {
                    if (!this.Visited.Contains(visit))
                    {
                        this.Visited.Add(visit);
                    }
                }
            }

            steps++;

            foreach (VectorCell<int, int> move in this.Map.AdjacentCardinal(point).Where(cell => !visited.Contains(cell.Point)))
            {
                this.FindPath(move.Point, steps, new(visited));
            }
        }

        public List<Vector<int>> GetShortestPath()
        {
            Vector<int> start = new(1, 1);

            Queue<Vector<int>> queue = new();
            HashSet<Vector<int>> visited = new();
            Dictionary<Vector<int>, Vector<int>> cameFrom = new();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Vector<int> current = queue.Dequeue();

                if (current == this.Target)
                {
                    return ReconstructPath(cameFrom, start, this.Target);
                }

                foreach (VectorCell<int, int> move in this.Map
                    .AdjacentCardinal(current)
                    .Where(cell => !visited.Contains(cell.Point)))
                {
                    visited.Add(move.Point);
                    cameFrom[move.Point] = current;
                    queue.Enqueue(move.Point);
                }
            }

            return [];
        }

        private List<PathNode> GetReachableSearch(int stepLimit)
        {
            Vector<int> start = new(1, 1);

            Queue<PathNode> queue = new();
            HashSet<Vector<int>> visited = [];
            List<PathNode> search = [];

            queue.Enqueue(new(start, 0));
            visited.Add(start);

            while (queue.Count > 0)
            {
                PathNode current = queue.Dequeue();
                search.Add(current);

                if (current.Steps == stepLimit)
                {
                    continue;
                }

                foreach (VectorCell<int, int> move in this.Map
                    .AdjacentCardinal(current.Point)
                    .Where(cell => !visited.Contains(cell.Point)))
                {
                    visited.Add(move.Point);
                    queue.Enqueue(new(move.Point, current.Steps + 1));
                }
            }

            return search;
        }

        private static List<Vector<int>> ReconstructPath(
            Dictionary<Vector<int>, Vector<int>> cameFrom,
            Vector<int> start,
            Vector<int> target)
        {
            List<Vector<int>> path = new();
            Vector<int> current = target;

            path.Add(current);

            while (current != start)
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> trail,
            HashSet<Vector<int>> searched,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);
            result.Add(this.BuildXAxis());

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();
                sb.Append((y % 10).ToString());
                sb.Append(' ');

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (point == new Vector<int>(1, 1))
                    {
                        sb.Append('S');
                    }
                    else if (point == this.Target)
                    {
                        sb.Append('X');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else if (searched.Contains(point))
                    {
                        sb.Append('o');
                    }
                    else if (this.Map.ContainsKey(point))
                    {
                        sb.Append('.');
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

        private string BuildXAxis()
        {
            StringBuilder sb = new();
            sb.Append("  ");

            for (int x = 0; x < this.Map.Width; x++)
            {
                sb.Append(x % 10);
            }

            return sb.ToString();
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
