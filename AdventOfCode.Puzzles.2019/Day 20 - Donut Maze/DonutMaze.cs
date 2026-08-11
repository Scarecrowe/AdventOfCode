namespace AdventOfCode.Puzzles._2019.Day_20___Donut_Maze
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class DonutMaze
    {
        public DonutMaze(string[] input)
        {
            this.Map = new(input, (c) => c);
            this.Portals = this.GetPortals().ToList();
            this.Start = this.Portals.First(c => c.Name == "AA");
        }

        public DonutMaze(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, char> Map { get; set; }

        private List<Portal> Portals { get; set; }

        private Portal Start { get; }

        private readonly record struct PathKey(Vector<int> Point, int Level);

        private sealed record PathNode(Vector<int> Point, int Distance, int Level);

        public int Search(bool recursive = false)
        {
            List<(List<Portal> Portals, bool[,] Visited)> states = new() { (this.Portals, new bool[this.Map.Height, this.Map.Width]) };
            Queue<(Vector<int> Point, int Distance, int Step)> queue = new();
            this.Start.Travelled = true;

            queue.Enqueue((new(this.Start.Point), 0, 0));

            while (queue.Any())
            {
                (Vector<int> point, int distance, int step) = queue.Dequeue();

                if (states[recursive ? step : 0].Visited[point.Y, point.X])
                {
                    continue;
                }

                states[recursive ? step : 0].Visited[point.Y, point.X] = true;

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(point))
                {
                    if (adjacent.Value == '.')
                    {
                        queue.Enqueue((new(adjacent.Point), distance + 1, step));
                    }
                    else if (char.IsLetter(adjacent.Value))
                    {
                        Portal? start = states[recursive ? step : 0].Portals.First(c => c.Point == point);

                        if (start.Travelled)
                        {
                            continue;
                        }

                        start.Travelled = true;

                        if (recursive)
                        {
                            if (step > 0 && (start.Name == "AA" || start.Name == "ZZ"))
                            {
                                continue;
                            }

                            if (start.Name == "ZZ")
                            {
                                return distance;
                            }

                            if (step == 0 && !start.Inner)
                            {
                                continue;
                            }
                        }

                        if (start.Name == "ZZ")
                        {
                            return distance;
                        }

                        Portal? end = states[recursive ? step : 0].Portals.First(c => c.Name == start.Name && c != start);

                        if (recursive
                            && start.Inner
                            && states.Count == step + 1)
                        {
                            List<Portal> portals = new();

                            foreach (Portal portal in this.Portals)
                            {
                                portals.Add(new(portal));
                            }

                            states.Add((portals, new bool[this.Map.Height, this.Map.Width]));
                        }

                        queue.Enqueue((end.Point, distance + 1, step + (start.Inner ? 1 : -1)));
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public DonutMaze RenderSilver(int renderEvery = 4)
        {
            return this.RenderPath(recursive: false, renderEvery);
        }

        public DonutMaze RenderGold(int renderEvery = 8)
        {
            return this.RenderPath(recursive: true, renderEvery);
        }

        private List<Portal> GetPortals()
        {
            List<Portal> portals = new();
            List<ProcessedCell<int, char>> cells = this.Map.Letters().Select(c => new ProcessedCell<int, char>(c)).ToList();

            foreach (ProcessedCell<int, char> cellA in cells.Where(c => !c.Processed))
            {
                List<VectorCell<int, char>> adjacentA = this.Map.AdjacentCardinal(cellA.Point).ToList();
                VectorCell<int, char>? portal = adjacentA.FirstOrDefault(c => c.Value == '.');

                foreach (ProcessedCell<int, char> cellB in cells.Where(c => !c.Processed))
                {
                    if (cellA == cellB)
                    {
                        continue;
                    }

                    if (adjacentA.Any(c => c.Point == cellB.Point))
                    {
                        if (portal != default)
                        {
                            portals.Add(new(portal.Point, $"{cellA.Value}{cellB.Value}", !(portal.Point.X == 2 || portal.Point.Y == 2 || portal.Point.X == this.Map.Width - 3 || portal.Point.Y == this.Map.Height - 3)));
                        }
                        else
                        {
                            portal = this.Map.AdjacentCardinal(cellB.Point).FirstOrDefault(c => c.Value == '.');
                            portals.Add(new(portal?.Point ?? new(0, 0), $"{cellA.Value}{cellB.Value}", !((portal?.Point.X ?? 0) == 2 || (portal?.Point.Y ?? 0) == 2 || (portal?.Point.X ?? 0) == this.Map.Width - 3 || (portal?.Point.Y ?? 0) == this.Map.Height - 3)));
                        }

                        cellA.Processed = true;
                        cellB.Processed = true;
                    }
                }
            }

            return portals;
        }

        private DonutMaze RenderPath(bool recursive, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<PathNode> path = this.FindPath(recursive);
            List<string[]> frames = [];
            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                PathNode node = path[i];
                trail.Add(node.Point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildFrame(
                        node.Point,
                        trail,
                        recursive
                            ? $"RECURSIVE DONUT MAZE // LEVEL {node.Level:000} // STEPS {node.Distance:0000}"
                            : $"DONUT MAZE // STEPS {node.Distance:0000}"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                PathNode last = path.Last();

                frames.Add(this.BuildFrame(
                    last.Point,
                    trail,
                    recursive
                        ? $"ZZ REACHED AT LEVEL 000 // STEPS {last.Distance:0000}"
                        : $"ZZ REACHED // STEPS {last.Distance:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<PathNode> FindPath(bool recursive)
        {
            Queue<PathNode> queue = new();
            HashSet<PathKey> visited = new();

            Dictionary<PathKey, PathKey> cameFrom = new();
            Dictionary<PathKey, PathNode> states = new();

            PathNode start = new(this.Start.Point, 0, 0);
            PathKey startKey = new(start.Point, start.Level);

            queue.Enqueue(start);
            states[startKey] = start;

            while (queue.Count > 0)
            {
                PathNode current = queue.Dequeue();
                PathKey currentKey = new(current.Point, current.Level);

                if (!visited.Add(currentKey))
                {
                    continue;
                }

                Portal? currentPortal = this.Portals.FirstOrDefault(p => p.Point == current.Point);

                if (currentPortal?.Name == "ZZ" && (!recursive || current.Level == 0))
                {
                    return ReconstructPath(cameFrom, states, currentKey);
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(current.Point))
                {
                    if (adjacent.Value != '.')
                    {
                        continue;
                    }

                    PathNode next = new(new(adjacent.Point), current.Distance + 1, current.Level);
                    PathKey nextKey = new(next.Point, next.Level);

                    if (!visited.Contains(nextKey) && !states.ContainsKey(nextKey))
                    {
                        states[nextKey] = next;
                        cameFrom[nextKey] = currentKey;
                        queue.Enqueue(next);
                    }
                }

                if (currentPortal == null ||
                    currentPortal.Name == "AA" ||
                    currentPortal.Name == "ZZ")
                {
                    continue;
                }

                int nextLevel = current.Level;

                if (recursive)
                {
                    nextLevel += currentPortal.Inner ? 1 : -1;

                    if (nextLevel < 0)
                    {
                        continue;
                    }
                }

                Portal destination = this.Portals.First(p =>
                    p.Name == currentPortal.Name &&
                    p.Point != currentPortal.Point);

                PathNode warped = new(destination.Point, current.Distance + 1, nextLevel);
                PathKey warpedKey = new(warped.Point, warped.Level);

                if (!visited.Contains(warpedKey) && !states.ContainsKey(warpedKey))
                {
                    states[warpedKey] = warped;
                    cameFrom[warpedKey] = currentKey;
                    queue.Enqueue(warped);
                }
            }

            return [];
        }

        private static List<PathNode> ReconstructPath(
            Dictionary<PathKey, PathKey> cameFrom,
            Dictionary<PathKey, PathNode> states,
            PathKey target)
        {
            List<PathNode> path = [];
            PathKey current = target;

            while (cameFrom.ContainsKey(current))
            {
                path.Add(states[current]);
                current = cameFrom[current];
            }

            path.Reverse();

            return path;
        }

        private string[] BuildFrame(
    Vector<int> current,
    HashSet<Vector<int>> trail,
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
                    char value = this.Map[y, x];

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else if (this.Portals.Any(p => p.Point == point && p.Name != "AA" && p.Name != "ZZ"))
                    {
                        sb.Append('*');
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
