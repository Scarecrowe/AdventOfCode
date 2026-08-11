namespace AdventOfCode.Puzzles._2019.Day_18___Many_Worlds_Interpretation
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;

    public class ManyWorldsInterpretation
    {
        public ManyWorldsInterpretation(string[] input)
        {
            this.Map = new(input, (c) => c);

            VectorCell<int, char>? cell = this.Map.FirstOrDefault('@');

            this.Locations = new() { new(cell?.Point ?? new(0, 0)) };
        }

        public ManyWorldsInterpretation(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public List<Vector<int>> Locations { get; }

        public HashSet<char>? Inventory { get; }

        public VectorArray<int, char> Map { get; }

        private readonly record struct PathStateKey(
            Vector<int> Location,
            string Visited);

        public ManyWorldsInterpretation SplitMap()
        {
            Vector<int> location = this.Locations[0];

            this.Map[this.Locations[0].Y, this.Locations[0].X - 1] = '#';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X] = '#';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X] = '#';
            this.Map[this.Locations[0].Y, this.Locations[0].X + 1] = '#';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X - 1] = '@';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X - 1] = '@';
            this.Map[this.Locations[0].Y - 1, this.Locations[0].X + 1] = '@';
            this.Map[this.Locations[0].Y + 1, this.Locations[0].X + 1] = '@';

            this.Map[this.Locations[0].Y, this.Locations[0].X] = '#';
            this.Locations.Clear();

            this.Locations.Add(location - 1);
            this.Locations.Add(location + new Vector<int>(-1, 1));
            this.Locations.Add(location + new Vector<int>(1, -1));
            this.Locations.Add(location + new Vector<int>(1, 1));

            return this;
        }

        public long CollectKeys(Vector<int>? location = null, List<bool>? visitedState = null)
        {
            location ??= this.Locations[0];

            Queue<QueueItem> queue = new();
            HashSet<(Vector<int> Point, string Visited)> cache = new();

            queue.Enqueue(new(location, 0, visitedState?.ToArray() ?? new bool[26]));

            while (queue.Count > 0)
            {
                QueueItem current = queue.Dequeue();

                var key = (new Vector<int>(current.Location), VisitedKey(current.Visited));

                if (cache.Contains(key))
                {
                    continue;
                }

                cache.Add(key);

                if (AllVisited(current.Visited))
                {
                    return current.Distance;
                }

                foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(current.Location).Where(c => c.Value != '#'))
                {
                    bool[] visited = (bool[])current.Visited.Clone();

                    if (IsDoor(adjacent.Value) && !current.Visited[adjacent.Value - 'A'])
                    {
                        continue;
                    }

                    if (IsKey(adjacent.Value))
                    {
                        visited[adjacent.Value - 'a'] = true;
                    }

                    queue.Enqueue(new(new(adjacent.Point), current.Distance + 1, visited));
                }
            }

            return -1;
        }

        public long CollectVaultKeys()
        {
            List<bool[]> visitedStart = new()
            {
                new bool[26],
                new bool[26],
                new bool[26],
                new bool[26]
            };

            this.IgnoreDoors(visitedStart);

            long result = 0;

            for (int i = 0; i < 4; i++)
            {
                result += this.CollectKeys(this.Locations[i], visitedStart[i].ToList());
            }

            return result;
        }

        public ManyWorldsInterpretation RenderSilver(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            bool[] initialVisited = new bool[26];

            List<QueueItem> path = this.FindCollectionPath(
                this.Locations[0],
                initialVisited);

            HashSet<Vector<int>> trail = [];

            int step = 0;

            foreach (QueueItem item in path)
            {
                trail.Add(item.Location);

                step++;

                if (step % renderEvery == 0 || AllVisited(item.Visited))
                {
                    frames.Add(this.BuildFrame(
                        item.Location,
                        item.Visited,
                        trail,
                        $"COLLECTING KEYS // STEPS {item.Distance:0000}"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                QueueItem last = path.Last();

                frames.Add(this.BuildFrame(
                    last.Location,
                    last.Visited,
                    trail,
                    $"ALL KEYS COLLECTED // STEPS {last.Distance:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public ManyWorldsInterpretation RenderGold(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            this.SplitMap();

            List<bool[]> visitedStart = new()
            {
                new bool[26],
                new bool[26],
                new bool[26],
                new bool[26]
            };

            this.IgnoreDoors(visitedStart);

            List<string[]> frames = [];
            HashSet<Vector<int>> trail = [];

            long totalSteps = 0;

            for (int i = 0; i < 4; i++)
            {
                List<QueueItem> path = this.FindCollectionPath(
                    this.Locations[i],
                    visitedStart[i]);

                int step = 0;

                foreach (QueueItem item in path)
                {
                    trail.Add(item.Location);

                    step++;

                    if (step % renderEvery == 0 || AllVisited(item.Visited))
                    {
                        frames.Add(this.BuildFrame(
                            item.Location,
                            item.Visited,
                            trail,
                            $"ROBOT {i + 1} COLLECTING KEYS // TOTAL {totalSteps + item.Distance:0000}"));
                    }
                }

                if (path.Count > 0)
                {
                    totalSteps += path.Last().Distance;
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    null,
                    Enumerable.Repeat(true, 26).ToArray(),
                    trail,
                    $"VAULT CLEARED // STEPS {totalSteps:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private static bool IsDoor(char value) => value >= 65 && value <= 90;

        private static bool IsKey(char value) => value >= 97 && value <= 122;

        private static bool AllVisited(bool[] visited)
        {
            for (int i = 0; i < 26; i++)
            {
                if (!visited[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string VisitedKey(bool[] visited)
        {
            StringBuilder sb = new();

            for (int i = 0; i < 26; i++)
            {
                sb.Append(visited[i] ? 1 : 0);
            }

            return sb.ToString();
        }

        private void IgnoreDoors(List<bool[]> visitedStart)
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (IsKey(this.Map[y, x]))
                    {
                        visitedStart[0][this.Map[y, x] - 'a'] = !((x <= this.Locations[0].X) && (y <= this.Locations[0].Y));
                        visitedStart[1][this.Map[y, x] - 'a'] = !((x <= this.Locations[1].X) && (y >= this.Locations[1].Y));
                        visitedStart[2][this.Map[y, x] - 'a'] = !((x >= this.Locations[2].X) && (y <= this.Locations[2].Y));
                        visitedStart[3][this.Map[y, x] - 'a'] = !((x >= this.Locations[3].X) && (y >= this.Locations[3].Y));
                    }
                }
            }
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            if (this.Renderer == null || frames.Count == 0)
            {
                return;
            }

            int width = frames
                .SelectMany(frame => frame)
                .Max(row => row.Length);

            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer.RenderFrame(
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

        private string[] BuildFrame(
    Vector<int>? robot,
    bool[] visited,
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

                    if (robot != null && point == robot)
                    {
                        sb.Append('@');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else if (value == '#')
                    {
                        sb.Append('#');
                    }
                    else if (value == '.')
                    {
                        sb.Append('.');
                    }
                    else if (value == '@')
                    {
                        sb.Append('+');
                    }
                    else if (IsKey(value))
                    {
                        sb.Append(visited[value - 'a'] ? '.' : '$');
                    }
                    else if (IsDoor(value))
                    {
                        sb.Append(visited[value - 'A'] ? '.' : '%');
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

        private List<QueueItem> FindCollectionPath(
    Vector<int> start,
    bool[] initialVisited)
        {
            Queue<QueueItem> queue = new();
            HashSet<PathStateKey> cache = new();

            Dictionary<PathStateKey, PathStateKey> cameFrom = new();
            Dictionary<PathStateKey, QueueItem> states = new();

            QueueItem startItem = new(start, 0, initialVisited);
            PathStateKey startKey = new(start, VisitedKey(initialVisited));

            queue.Enqueue(startItem);
            states[startKey] = startItem;

            while (queue.Count > 0)
            {
                QueueItem current = queue.Dequeue();

                PathStateKey currentKey = new(
                    new Vector<int>(current.Location),
                    VisitedKey(current.Visited));

                if (cache.Contains(currentKey))
                {
                    continue;
                }

                cache.Add(currentKey);

                if (AllVisited(current.Visited))
                {
                    return ReconstructCollectionPath(cameFrom, states, currentKey);
                }

                foreach (VectorCell<int, char> adjacent in this.Map
                    .AdjacentCardinal(current.Location)
                    .Where(c => c.Value != '#'))
                {
                    bool[] visited = (bool[])current.Visited.Clone();

                    if (IsDoor(adjacent.Value) && !visited[adjacent.Value - 'A'])
                    {
                        continue;
                    }

                    if (IsKey(adjacent.Value))
                    {
                        visited[adjacent.Value - 'a'] = true;
                    }

                    QueueItem next = new(
                        new(adjacent.Point),
                        current.Distance + 1,
                        visited);

                    PathStateKey nextKey = new(
                        new Vector<int>(next.Location),
                        VisitedKey(next.Visited));

                    if (cache.Contains(nextKey))
                    {
                        continue;
                    }

                    if (!states.ContainsKey(nextKey))
                    {
                        states[nextKey] = next;
                        cameFrom[nextKey] = currentKey;
                        queue.Enqueue(next);
                    }
                }
            }

            return [];
        }

        private static List<QueueItem> ReconstructCollectionPath(
            Dictionary<PathStateKey, PathStateKey> cameFrom,
            Dictionary<PathStateKey, QueueItem> states,
            PathStateKey target)
        {
            List<QueueItem> path = [];

            PathStateKey current = target;

            while (cameFrom.ContainsKey(current))
            {
                path.Add(states[current]);
                current = cameFrom[current];
            }

            path.Reverse();

            return path;
        }
    }
}
