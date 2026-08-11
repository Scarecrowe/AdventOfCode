namespace AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class BlizzardBasin
    {
        public BlizzardBasin(string[] input)
        {
            this.Start = new(0, 0);
            this.Finish = new(0, 0);
            this.Maps = new();
            this.Parse(input);
        }

        public BlizzardBasin(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private Vector<int> Start { get; set; }

        private Vector<int> Finish { get; set; }

        private BlizzardBasinMaps Maps { get; set; }

        private string[] Input { get; set; } = [];

        private List<Blizzard> InitialBlizzards { get; set; } = [];

        private readonly record struct PathKey(Vector<int> Point, int MinuteMod);

        private sealed record PathNode(Vector<int> Point, int Minutes);

        public int FewestMinutes()
            => new BlizzardBasinState(0, this.Start).Move(this.Finish, this.Maps).Minutes;

        public int FewestMinutesWithRoundTrip()
            => new BlizzardBasinState(0, this.Start).Move(this.Finish, this.Maps).Move(this.Start, this.Maps).Move(this.Finish, this.Maps).Minutes;

        public BlizzardBasin RenderSilver(int renderEvery = 2)
        {
            return this.RenderPath(
                this.FindPath(this.Start, this.Finish, 0),
                "BLIZZARD BASIN",
                renderEvery);
        }

        public BlizzardBasin RenderGold(int renderEvery = 3)
        {
            List<PathNode> there = this.FindPath(this.Start, this.Finish, 0);
            List<PathNode> back = this.FindPath(this.Finish, this.Start, there.Last().Minutes);
            List<PathNode> again = this.FindPath(this.Start, this.Finish, back.Last().Minutes);

            return this.RenderPath(
                there.Concat(back.Skip(1)).Concat(again.Skip(1)).ToList(),
                "BLIZZARD BASIN ROUND TRIP",
                renderEvery);
        }

        private void Parse(string[] input)
        {
            this.Input = input;
            this.InitialBlizzards = Blizzard.Parse(input);

            List<VectorArray<int, BlizzardBasinType>> maps = new();
            List<Blizzard> blizzards = Blizzard.Parse(input);
            VectorArray<int, BlizzardBasinType> map = BlizzardBasinMaps.Build(input, blizzards);
            string initialKey = map.ToString((c) => (char)c);

            while (true)
            {
                maps.Add(map);
                blizzards = blizzards.Select(b => b.Move()).ToList();
                map = BlizzardBasinMaps.Build(input, blizzards);

                if (map.ToString((c) => (char)c) == initialKey)
                {
                    break;
                }
            }

            this.Maps = new BlizzardBasinMaps(maps);
            this.Start = new Vector<int>(1, 0);
            this.Finish = new Vector<int>(this.Maps.Point.X - 2, this.Maps.Point.Y - 1);
        }

        private List<PathNode> FindPath(Vector<int> start, Vector<int> finish, int startMinute)
        {
            PriorityQueue<PathNode, int> queue = new();
            HashSet<PathKey> visited = [];
            Dictionary<PathNode, PathNode> cameFrom = [];

            PathNode first = new(start, startMinute);
            queue.Enqueue(first, first.Minutes + (int)finish.Distance(start));

            while (queue.Count > 0)
            {
                PathNode current = queue.Dequeue();
                PathKey key = new(current.Point, current.Minutes % this.Maps.Period);

                if (!visited.Add(key))
                {
                    continue;
                }

                if (current.Point == finish)
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (Vector<int> next in Directions(current.Point))
                {
                    int nextMinute = current.Minutes + 1;

                    if (this.Maps.GetMap(nextMinute, next) != BlizzardBasinType.Empty)
                    {
                        continue;
                    }

                    PathNode node = new(next, nextMinute);

                    if (!cameFrom.ContainsKey(node))
                    {
                        cameFrom[node] = current;
                        queue.Enqueue(node, nextMinute + (int)finish.Distance(next));
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private static List<PathNode> ReconstructPath(
            Dictionary<PathNode, PathNode> cameFrom,
            PathNode target)
        {
            List<PathNode> path = [target];
            PathNode current = target;

            while (cameFrom.TryGetValue(current, out PathNode? previous))
            {
                path.Add(previous);
                current = previous;
            }

            path.Reverse();

            return path;
        }

        private BlizzardBasin RenderPath(List<PathNode> path, string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                PathNode node = path[i];
                trail.Add(node.Point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    this.Renderer.RenderFrame(
                        new Frame(this.BuildFrame(
                            node.Point,
                            trail,
                            node.Minutes,
                            $"{title} // MINUTE {node.Minutes:0000}")));
                }
            }

            return this;
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> trail,
            int minute,
            string title)
        {
            Dictionary<Vector<int>, List<char>> blizzards = this.GetBlizzardsAt(minute);

            List<string> result =
            [
                title,
                string.Empty
            ];

            for (int y = 0; y < this.Input.Length; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Input[y].Length; x++)
                {
                    Vector<int> point = new(x, y);
                    char map = this.Input[y][x];

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (map == '#')
                    {
                        sb.Append('#');
                    }
                    else if (blizzards.TryGetValue(point, out List<char>? chars))
                    {
                        sb.Append(chars.Count == 1 ? chars[0] : chars.Count > 9 ? '*' : chars.Count.ToString()[0]);
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private Dictionary<Vector<int>, List<char>> GetBlizzardsAt(int minute)
        {
            Dictionary<Vector<int>, List<char>> result = [];

            foreach (Blizzard blizzard in this.InitialBlizzards)
            {
                int x = 1 + Mod(blizzard.Point.X - 1 + blizzard.Direction.X * minute, this.Maps.Point.X - 2);
                int y = 1 + Mod(blizzard.Point.Y - 1 + blizzard.Direction.Y * minute, this.Maps.Point.Y - 2);

                Vector<int> point = new(x, y);

                if (!result.ContainsKey(point))
                {
                    result[point] = [];
                }

                result[point].Add(blizzard.Symbol);
            }

            return result;
        }

        private static List<Vector<int>> Directions(Vector<int> point)
        {
            return
            [
                point,
                new(point.X, point.Y - 1),
                new(point.X, point.Y + 1),
                new(point.X - 1, point.Y),
                new(point.X + 1, point.Y)
            ];
        }

        private static int Mod(int value, int divisor)
        {
            int result = value % divisor;

            return result < 0 ? result + divisor : result;
        }
    }
}