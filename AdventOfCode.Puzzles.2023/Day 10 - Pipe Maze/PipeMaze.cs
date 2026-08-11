namespace AdventOfCode.Puzzles._2023.Day_10___Pipe_Maze
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class PipeMaze
    {
        public PipeMaze(string[] input)
        {
            this.Map = new(input, value => value);
            this.Start = this.FindStart();
            this.StartPipe = this.ResolveStartPipe();
        }

        public PipeMaze(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, char> Map { get; }

        private Vector<int> Start { get; }

        private char StartPipe { get; }

        private readonly record struct FloodStep(Vector<int> Point);

        public long Move()
            => this.GetOrderedLoop().Count / 2;

        public long CountInside()
        {
            List<Vector<int>> loop = this.GetOrderedLoop();
            HashSet<Vector<int>> loopSet = [.. loop];

            HashSet<(int X, int Y)> blocked = this.BuildExpandedLoop(loopSet);
            HashSet<(int X, int Y)> outside = this.FloodExpandedOutside(blocked);

            int count = 0;

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (loopSet.Contains(point))
                    {
                        continue;
                    }

                    if (!outside.Contains((x * 3 + 1, y * 3 + 1)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public PipeMaze RenderSilver(int renderEvery = 6)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<int>> loop = this.GetOrderedLoop();
            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < loop.Count; i++)
            {
                trail.Add(loop[i]);

                if (i % renderEvery == 0 || i == loop.Count - 1)
                {
                    this.RenderFrame(this.BuildFrame(
                        current: loop[i],
                        trail: trail,
                        outside: [],
                        inside: [],
                        title: $"PIPE MAZE // LOOP {i + 1:00000}/{loop.Count:00000} // FARTHEST {loop.Count / 2:0000}"));
                }
            }

            return this;
        }

        public PipeMaze RenderGold(int renderEvery = 30)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<int>> loop = this.GetOrderedLoop();
            HashSet<Vector<int>> loopSet = [.. loop];

            HashSet<(int X, int Y)> blocked = this.BuildExpandedLoop(loopSet);
            HashSet<(int X, int Y)> outsideExpanded = [];
            HashSet<Vector<int>> outside = [];
            HashSet<Vector<int>> inside = [];

            int step = 0;

            foreach ((int X, int Y) expanded in this.FloodExpandedOutsideSteps(blocked))
            {
                outsideExpanded.Add(expanded);

                if (expanded.X % 3 == 1 && expanded.Y % 3 == 1)
                {
                    Vector<int> original = new(expanded.X / 3, expanded.Y / 3);

                    if (!loopSet.Contains(original))
                    {
                        outside.Add(original);
                    }
                }

                step++;

                if (step % renderEvery == 0)
                {
                    this.RenderFrame(this.BuildFrame(
                        current: new(
                            Math.Clamp(expanded.X / 3, 0, this.Map.Width - 1),
                            Math.Clamp(expanded.Y / 3, 0, this.Map.Height - 1)),
                        trail: loopSet,
                        outside: outside,
                        inside: inside,
                        title: $"PIPE MAZE // SCANNING OUTSIDE // CHECKED {step:00000}"));
                }
            }

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (loopSet.Contains(point))
                    {
                        continue;
                    }

                    if (!outsideExpanded.Contains((x * 3 + 1, y * 3 + 1)))
                    {
                        inside.Add(point);

                        this.RenderFrame(this.BuildFrame(
                            current: point,
                            trail: loopSet,
                            outside: outside,
                            inside: inside,
                            title: $"PIPE MAZE // ENCLOSED {inside.Count:0000}"));
                    }
                }
            }

            return this;
        }

        private Vector<int> FindStart()
        {
            foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == 'S')
                {
                    return new(cell.Point);
                }
            }

            throw new InvalidOperationException("Start tile not found.");
        }

        private char ResolveStartPipe()
        {
            List<Cardinal> directions = [];

            foreach (VectorCell<int, char> adjacent in this.Map.AdjacentCardinal(this.Start))
            {
                if (Connects(adjacent.Value, Opposite(adjacent.Direction)))
                {
                    directions.Add(adjacent.Direction);
                }
            }

            if (directions.Contains(Cardinal.North) && directions.Contains(Cardinal.South))
            {
                return '|';
            }

            if (directions.Contains(Cardinal.East) && directions.Contains(Cardinal.West))
            {
                return '-';
            }

            if (directions.Contains(Cardinal.North) && directions.Contains(Cardinal.East))
            {
                return 'L';
            }

            if (directions.Contains(Cardinal.North) && directions.Contains(Cardinal.West))
            {
                return 'J';
            }

            if (directions.Contains(Cardinal.South) && directions.Contains(Cardinal.West))
            {
                return '7';
            }

            if (directions.Contains(Cardinal.South) && directions.Contains(Cardinal.East))
            {
                return 'F';
            }

            throw new InvalidOperationException("Could not resolve start pipe.");
        }

        private List<Vector<int>> GetOrderedLoop()
        {
            List<Vector<int>> loop = [this.Start];

            VectorCell<int, char> first = this.Map
                .AdjacentCardinal(this.Start)
                .First(adjacent =>
                    Connects(this.StartPipe, adjacent.Direction) &&
                    Connects(adjacent.Value, Opposite(adjacent.Direction)));

            Vector<int> previous = this.Start;
            Vector<int> current = new(first.Point);

            while (current != this.Start)
            {
                loop.Add(current);

                char currentPipe = this.GetPipe(current);

                VectorCell<int, char> next = this.Map
                    .AdjacentCardinal(current)
                    .First(adjacent =>
                        adjacent.Point != previous &&
                        Connects(currentPipe, adjacent.Direction) &&
                        Connects(this.GetPipe(adjacent.Point), Opposite(adjacent.Direction)));

                previous = current;
                current = new(next.Point);
            }

            return loop;
        }

        private char GetPipe(Vector<int> point)
        {
            char value = this.Map[point.Y, point.X];

            return value == 'S'
                ? this.StartPipe
                : value;
        }

        private HashSet<(int X, int Y)> BuildExpandedLoop(HashSet<Vector<int>> loop)
        {
            HashSet<(int X, int Y)> blocked = [];

            foreach (Vector<int> point in loop)
            {
                char pipe = this.GetPipe(point);

                int x = point.X * 3 + 1;
                int y = point.Y * 3 + 1;

                blocked.Add((x, y));

                if (Connects(pipe, Cardinal.North))
                {
                    blocked.Add((x, y - 1));
                }

                if (Connects(pipe, Cardinal.South))
                {
                    blocked.Add((x, y + 1));
                }

                if (Connects(pipe, Cardinal.West))
                {
                    blocked.Add((x - 1, y));
                }

                if (Connects(pipe, Cardinal.East))
                {
                    blocked.Add((x + 1, y));
                }
            }

            return blocked;
        }

        private HashSet<(int X, int Y)> FloodExpandedOutside(HashSet<(int X, int Y)> blocked)
            => [.. this.FloodExpandedOutsideSteps(blocked)];

        private IEnumerable<(int X, int Y)> FloodExpandedOutsideSteps(HashSet<(int X, int Y)> blocked)
        {
            int width = this.Map.Width * 3;
            int height = this.Map.Height * 3;

            Queue<(int X, int Y)> queue = [];
            HashSet<(int X, int Y)> visited = [];

            for (int x = 0; x < width; x++)
            {
                queue.Enqueue((x, 0));
                queue.Enqueue((x, height - 1));
            }

            for (int y = 0; y < height; y++)
            {
                queue.Enqueue((0, y));
                queue.Enqueue((width - 1, y));
            }

            while (queue.Count > 0)
            {
                (int X, int Y) current = queue.Dequeue();

                if (current.X < 0 ||
                    current.Y < 0 ||
                    current.X >= width ||
                    current.Y >= height ||
                    blocked.Contains(current) ||
                    !visited.Add(current))
                {
                    continue;
                }

                yield return current;

                queue.Enqueue((current.X, current.Y - 1));
                queue.Enqueue((current.X, current.Y + 1));
                queue.Enqueue((current.X - 1, current.Y));
                queue.Enqueue((current.X + 1, current.Y));
            }
        }

        private string[] BuildFrame(
            Vector<int> current,
            HashSet<Vector<int>> trail,
            HashSet<Vector<int>> outside,
            HashSet<Vector<int>> inside,
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

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (inside.Contains(point))
                    {
                        sb.Append('I');
                    }
                    else if (outside.Contains(point))
                    {
                        sb.Append('O');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append(this.GetPipe(point));
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

        private void RenderFrame(string[] frame)
        {
            this.Renderer?.RenderFrame(new Frame(frame));
        }

        private static bool Connects(char pipe, Cardinal direction)
        {
            return pipe switch
            {
                '|' => direction is Cardinal.North or Cardinal.South,
                '-' => direction is Cardinal.East or Cardinal.West,
                'L' => direction is Cardinal.North or Cardinal.East,
                'J' => direction is Cardinal.North or Cardinal.West,
                '7' => direction is Cardinal.South or Cardinal.West,
                'F' => direction is Cardinal.South or Cardinal.East,
                _ => false
            };
        }

        private static Cardinal Opposite(Cardinal direction)
        {
            return direction switch
            {
                Cardinal.North => Cardinal.South,
                Cardinal.South => Cardinal.North,
                Cardinal.East => Cardinal.West,
                Cardinal.West => Cardinal.East,
                _ => throw new InvalidOperationException()
            };
        }
    }
}