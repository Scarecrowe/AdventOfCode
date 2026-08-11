namespace AdventOfCode.Puzzles._2016.Day_24___Air_Duct_Spelunking
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class AirDuctSpelunking
    {
        public AirDuctSpelunking(string[] input)
        {
            this.InterfaceCount--;
            this.Map = new(input, (c) =>
            {
                if (char.IsNumber(c) && c > this.InterfaceCount)
                {
                    this.InterfaceCount++;
                }

                return c;
            });
        }

        public AirDuctSpelunking(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public VectorDictionary<int, char> Map { get; }

        public int InterfaceCount { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private sealed record RouteFrame(
            Vector<int> Point,
            long Step,
            int CurrentTarget,
            int VisitedCount,
            int TotalTargets);

        public long ShortestPath(bool reset = false)
        {
            long result = long.MaxValue;

            List<List<int>> permutations = this.GeneratePermutations();
            Dictionary<int, Dictionary<int, long>> paths = this.GeneratePaths();

            for (int j = 0; j < permutations.Count; j++)
            {
                List<int> permutation = permutations[j];

                long current = paths[0][permutation[0]];

                for (int i = 0; i < permutation.Count - 1; i++)
                {
                    current += paths[permutation[i]][permutation[i + 1]];
                }

                if (current < result && reset)
                {
                    current += paths[permutation.Last()][0];
                }

                result = Math.Min(result, current);
            }

            return result;
        }

        public AirDuctSpelunking RenderSilver(int renderEvery = 2)
        {
            return this.RenderPath(reset: false, renderEvery);
        }

        public AirDuctSpelunking RenderGold(int renderEvery = 3)
        {
            return this.RenderPath(reset: true, renderEvery);
        }

        private Dictionary<int, Dictionary<int, long>> GeneratePaths()
        {
            Dictionary<int, Dictionary<int, long>> distances = new();

            foreach (Vector<int> point in Vector<int>.AxisEnumerator(this.InterfaceCount + 1, this.InterfaceCount + 1))
            {
                Vector<int> source = this.Map.FirstOrDefault(x => x.Value == point.Y.ToString()[0]).Key;
                Vector<int> destination = this.Map.FirstOrDefault(x => x.Value == point.X.ToString()[0]).Key;

                if (!distances.ContainsKey(point.Y))
                {
                    distances.Add(point.Y, new());
                }

                distances[point.Y].Add(point.X, this.FindPath(source, destination));
            }

            return distances;
        }

        private long FindPath(Vector<int> source, Vector<int> destination)
        {
            long result = long.MaxValue;
            bool[,] visited = new bool[this.Map.Width, this.Map.Height];
            Queue<(Vector<int> point, int count)> queue = new();

            queue.Enqueue((source, 0));

            while (queue.Count > 0)
            {
                (Vector<int> point, int count) = queue.Dequeue();
                count++;

                if (count > result)
                {
                    continue;
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(point))
                {
                    if (visited[adjacent.Point.X, adjacent.Point.Y] || adjacent.Value == '#')
                    {
                        continue;
                    }

                    if (adjacent.Point == destination)
                    {
                        result = Math.Min(result, count);
                        break;
                    }

                    visited[adjacent.Point.X, adjacent.Point.Y] = true;

                    queue.Enqueue((adjacent.Point, count));
                }
            }

            return result;
        }

        private List<List<int>> GeneratePermutations()
        {
            List<int> interfaces = new();

            for (int i = 1; i <= this.InterfaceCount; i++)
            {
                interfaces.Add(i);
            }

            return interfaces.Permutations(this.InterfaceCount);
        }

        private List<Vector<int>> FindPathPoints(Vector<int> source, Vector<int> destination)
        {
            Queue<Vector<int>> queue = new();
            HashSet<Vector<int>> visited = new();
            Dictionary<Vector<int>, Vector<int>> cameFrom = new();

            queue.Enqueue(source);
            visited.Add(source);

            while (queue.Count > 0)
            {
                Vector<int> current = queue.Dequeue();

                if (current == destination)
                {
                    return ReconstructPath(cameFrom, source, destination);
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(current))
                {
                    if (visited.Contains(adjacent.Point) || adjacent.Value == '#')
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
            Vector<int> source,
            Vector<int> destination)
        {
            List<Vector<int>> path = new();
            Vector<int> current = destination;

            path.Add(current);

            while (current != source)
            {
                current = cameFrom[current];
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        private Vector<int> GetInterfacePoint(int id)
        {
            return this.Map.First(x => x.Value == id.ToString()[0]).Key;
        }

        public List<Vector<int>> ShortestPathPoints(bool reset = false)
        {
            List<int> bestPermutation = [];
            long bestDistance = long.MaxValue;

            List<List<int>> permutations = this.GeneratePermutations();
            Dictionary<int, Dictionary<int, long>> paths = this.GeneratePaths();

            foreach (List<int> permutation in permutations)
            {
                long current = paths[0][permutation[0]];

                for (int i = 0; i < permutation.Count - 1; i++)
                {
                    current += paths[permutation[i]][permutation[i + 1]];
                }

                if (reset)
                {
                    current += paths[permutation.Last()][0];
                }

                if (current < bestDistance)
                {
                    bestDistance = current;
                    bestPermutation = permutation;
                }
            }

            List<int> route = new() { 0 };
            route.AddRange(bestPermutation);

            if (reset)
            {
                route.Add(0);
            }

            List<Vector<int>> points = [];

            for (int i = 0; i < route.Count - 1; i++)
            {
                Vector<int> source = this.GetInterfacePoint(route[i]);
                Vector<int> destination = this.GetInterfacePoint(route[i + 1]);

                List<Vector<int>> section = this.FindPathPoints(source, destination);

                if (points.Count > 0)
                {
                    section.RemoveAt(0);
                }

                points.AddRange(section);
            }

            return points;
        }

        private AirDuctSpelunking RenderPath(bool reset, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<int> route = this.ShortestRoute(reset);
            List<RouteFrame> routeFrames = this.BuildRouteFrames(route);
            List<string[]> frames = [];
            HashSet<Vector<int>> trail = [];
            HashSet<int> visitedInterfaces = [];

            for (int i = 0; i < routeFrames.Count; i++)
            {
                RouteFrame routeFrame = routeFrames[i];
                trail.Add(routeFrame.Point);

                char mapValue = this.Map[routeFrame.Point];

                if (char.IsNumber(mapValue))
                {
                    visitedInterfaces.Add(mapValue - '0');
                }

                if (i % renderEvery == 0 || i == routeFrames.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        routeFrame.Point,
                        trail,
                        visitedInterfaces,
                        reset
                            ? $"AIR DUCT SPELUNKING // RETURN TO 0 // STEPS {routeFrame.Step:0000} // TARGET {routeFrame.CurrentTarget}"
                            : $"AIR DUCT SPELUNKING // STEPS {routeFrame.Step:0000} // TARGET {routeFrame.CurrentTarget}"));
                }
            }

            RouteFrame last = routeFrames.Last();
            string done = reset
                ? $"ALL WIRES BYPASSED AND ROBOT RETURNED // STEPS {last.Step:0000}"
                : $"ALL WIRES BYPASSED // STEPS {last.Step:0000}";

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    last.Point,
                    trail,
                    visitedInterfaces,
                    done));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<int> ShortestRoute(bool reset)
        {
            List<int> bestPermutation = [];
            long bestDistance = long.MaxValue;

            List<List<int>> permutations = this.GeneratePermutations();
            Dictionary<int, Dictionary<int, long>> paths = this.GeneratePaths();

            foreach (List<int> permutation in permutations)
            {
                long current = paths[0][permutation[0]];

                for (int i = 0; i < permutation.Count - 1; i++)
                {
                    current += paths[permutation[i]][permutation[i + 1]];
                }

                if (reset)
                {
                    current += paths[permutation.Last()][0];
                }

                if (current < bestDistance)
                {
                    bestDistance = current;
                    bestPermutation = permutation;
                }
            }

            List<int> route = new() { 0 };
            route.AddRange(bestPermutation);

            if (reset)
            {
                route.Add(0);
            }

            return route;
        }

        private List<RouteFrame> BuildRouteFrames(List<int> route)
        {
            List<RouteFrame> frames = [];
            long step = 0;
            int visited = 1;

            for (int i = 0; i < route.Count - 1; i++)
            {
                Vector<int> source = this.GetInterfacePoint(route[i]);
                Vector<int> destination = this.GetInterfacePoint(route[i + 1]);
                List<Vector<int>> section = this.FindPathPoints(source, destination);

                if (i > 0 && section.Count > 0)
                {
                    section.RemoveAt(0);
                }

                foreach (Vector<int> point in section)
                {
                    frames.Add(new RouteFrame(
                        point,
                        step,
                        route[i + 1],
                        visited,
                        this.InterfaceCount + 1));

                    step++;
                }

                visited = Math.Min(this.InterfaceCount + 1, visited + 1);
            }

            if (frames.Count > 0)
            {
                RouteFrame last = frames.Last();
                frames[^1] = last with { Step = step - 1 };
            }

            return frames;
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> trail,
            HashSet<int> visitedInterfaces,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Map[point];

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (char.IsNumber(value) && visitedInterfaces.Contains(value - '0'))
                    {
                        sb.Append('✓');
                    }
                    else if (char.IsNumber(value))
                    {
                        sb.Append(value);
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
