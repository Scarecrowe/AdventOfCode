namespace AdventOfCode.Puzzles._2016.Day_17___Two_Steps_Forward
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class TwoStepsForward
    {
        public TwoStepsForward(string hash)
        {
            this.Vault = CreateVault();
            this.Start = new(1, 1);
            this.Hash = hash;
        }

        public TwoStepsForward(string hash, IFrameRenderer renderer)
            : this(hash)
        {
            this.Renderer = renderer;
        }

        public string Hash { get; }

        public VectorArray<int, EntityType> Vault { get; }

        public Vector<int> Start { get; set; }

        public IFrameRenderer? Renderer { get; }

        private static List<char> Open { get; } = new() { 'b', 'c', 'd', 'e', 'f' };

        private static readonly Vector<int> TargetRoom = new(7, 7);

        public int LongestPathLength() => this.LongestPath().Length;

        private static VectorArray<int, EntityType> CreateVault()
        {
            string[] vault = new string[9];

            vault[0] = "#########";
            vault[1] = "#S| | | #";
            vault[2] = "#-#-#-#-#";
            vault[3] = "# | | | #";
            vault[4] = "#-#-#-#-#";
            vault[5] = "# | | | #";
            vault[6] = "#-#-#-#-#";
            vault[7] = "# | | |  ";
            vault[8] = "####### V";

            return new(vault, "#-|SV ");
        }

        public string LongestPath()
        {
            Queue<(Vector<int> Point, string Path)> queue = new();

            queue.Enqueue((new Vector<int>(1, 1), string.Empty));

            string longest = string.Empty;

            while (queue.Count > 0)
            {
                (Vector<int> Point, string Path) current = queue.Dequeue();

                if (current.Point == TargetRoom)
                {
                    if (current.Path.Length > longest.Length)
                    {
                        longest = current.Path;
                    }

                    continue;
                }

                string hash = $"{this.Hash}{current.Path}"
                    .ToMd5()
                    .ToLower()[..4];

                if (Open.Contains(hash[0]) && current.Point.Y > 1)
                {
                    queue.Enqueue((new(current.Point.X, current.Point.Y - 2), $"{current.Path}U"));
                }

                if (Open.Contains(hash[1]) && current.Point.Y < 7)
                {
                    queue.Enqueue((new(current.Point.X, current.Point.Y + 2), $"{current.Path}D"));
                }

                if (Open.Contains(hash[2]) && current.Point.X > 1)
                {
                    queue.Enqueue((new(current.Point.X - 2, current.Point.Y), $"{current.Path}L"));
                }

                if (Open.Contains(hash[3]) && current.Point.X < 7)
                {
                    queue.Enqueue((new(current.Point.X + 2, current.Point.Y), $"{current.Path}R"));
                }
            }

            return longest;
        }

        public string ShortestPath()
        {
            Queue<(Vector<int> Point, string Path)> queue = new();

            queue.Enqueue((new Vector<int>(1, 1), string.Empty));

            while (queue.Count > 0)
            {
                (Vector<int> Point, string Path) current = queue.Dequeue();

                if (current.Point == TargetRoom)
                {
                    return current.Path;
                }

                string hash = $"{this.Hash}{current.Path}"
                    .ToMd5()
                    .ToLower()[..4];

                if (Open.Contains(hash[0]) && current.Point.Y > 1)
                {
                    queue.Enqueue((new(current.Point.X, current.Point.Y - 2), current.Path + "U"));
                }

                if (Open.Contains(hash[1]) && current.Point.Y < 7)
                {
                    queue.Enqueue((new(current.Point.X, current.Point.Y + 2), current.Path + "D"));
                }

                if (Open.Contains(hash[2]) && current.Point.X > 1)
                {
                    queue.Enqueue((new(current.Point.X - 2, current.Point.Y), current.Path + "L"));
                }

                if (Open.Contains(hash[3]) && current.Point.X < 7)
                {
                    queue.Enqueue((new(current.Point.X + 2, current.Point.Y), current.Path + "R"));
                }
            }

            throw new InvalidOperationException("No path to the vault was found.");
        }

        public TwoStepsForward RenderSilver(int holdFrames = 24)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            string path = this.ShortestPath();

            List<string[]> frames = this.BuildFrames(path, false);

            for (int i = 0; i < holdFrames; i++)
            {
                frames.Add(frames.Last());
            }

            this.RenderFrames(frames);

            return this;
        }

        public TwoStepsForward RenderGold(int skip = 8, int holdFrames = 24)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            string path = this.LongestPath();

            List<string[]> frames = this.BuildFrames(path, true, skip);

            for (int i = 0; i < holdFrames; i++)
            {
                frames.Add(frames.Last());
            }

            this.RenderFrames(frames);

            return this;
        }

        private List<string[]> BuildFrames(string path, bool gold, int skip = 1)
        {
            List<string[]> frames = [];

            Vector<int> current = new(1, 1);

            List<Vector<int>> visited = [current];

            frames.Add(this.BuildFrame(
                current,
                visited,
                string.Empty,
                gold));

            for (int i = 0; i < path.Length; i++)
            {
                char move = path[i];

                Vector<int> next = Move(current, move);

                Vector<int> door = DoorBetween(current, next);

                visited.Add(door);
                visited.Add(next);

                current = next;

                if (i % skip == 0 || i == path.Length - 1)
                {
                    frames.Add(this.BuildFrame(
                        current,
                        visited,
                        path[..(i + 1)],
                        gold));
                }
            }

            return frames;
        }

        private string[] BuildFrame(
            Vector<int> current,
            List<Vector<int>> visited,
            string path,
            bool gold)
        {
            List<string> result = [];

            string hash = $"{this.Hash}{path}"
                .ToMd5()
                .ToLower()[..4];

            bool north = Open.Contains(hash[0]);
            bool south = Open.Contains(hash[1]);
            bool west = Open.Contains(hash[2]);
            bool east = Open.Contains(hash[3]);

            result.Add(
                gold
                    ? $"TWO STEPS FORWARD GOLD // LENGTH {path.Length:000}"
                    : $"TWO STEPS FORWARD // STEPS {path.Length:000}");

            result.Add($"PATH {path}");
            result.Add($"HASH {hash}");
            result.Add(string.Empty);

            for (int y = 0; y < 9; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < 9; x++)
                {
                    Vector<int> point = new(x, y);

                    char c = this.GetCharacter(
                        point,
                        current,
                        visited,
                        north,
                        south,
                        west,
                        east);

                    sb.Append(c);
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private char GetCharacter(
            Vector<int> point,
            Vector<int> current,
            List<Vector<int>> visited,
            bool north,
            bool south,
            bool west,
            bool east)
        {
            if (point == current)
            {
                return '@';
            }

            if (point == TargetRoom)
            {
                return 'V';
            }

            EntityType type = this.Vault[point];

            if (type == EntityType.Wall)
            {
                return '#';
            }

            if (type == EntityType.Space)
            {
                return visited.Contains(point)
                    ? '~'
                    : '.';
            }

            if (type == EntityType.DoorHorizontal ||
                type == EntityType.DoorVertical)
            {
                bool open = IsDoorOpen(
                    point,
                    current,
                    north,
                    south,
                    west,
                    east);

                if (!open)
                {
                    return '#';
                }

                return visited.Contains(point)
                    ? '~'
                    : '+';
            }

            if (type == EntityType.Vault)
            {
                return 'V';
            }

            return ' ';
        }

        private static Vector<int> DoorBetween(Vector<int> a, Vector<int> b)
        {
            return new(
                (a.X + b.X) / 2,
                (a.Y + b.Y) / 2);
        }

        private static Vector<int> Move(Vector<int> point, char move)
        {
            return move switch
            {
                'U' => new(point.X, point.Y - 2),
                'D' => new(point.X, point.Y + 2),
                'L' => new(point.X - 2, point.Y),
                'R' => new(point.X + 2, point.Y),
                _ => point
            };
        }

        private static bool IsDoorOpen(
            Vector<int> door,
            Vector<int> room,
            bool north,
            bool south,
            bool west,
            bool east)
        {
            if (door.X == room.X && door.Y == room.Y - 1)
            {
                return north;
            }

            if (door.X == room.X && door.Y == room.Y + 1)
            {
                return south;
            }

            if (door.X == room.X - 1 && door.Y == room.Y)
            {
                return west;
            }

            if (door.X == room.X + 1 && door.Y == room.Y)
            {
                return east;
            }

            return false;
        }

        private void RenderFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(c => c).Max(c => c.Length);
            int height = frames.Max(c => c.Length);

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