namespace AdventOfCode.Puzzles._2024.Day_06___Guard_Gallivant
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GuardGallivant
    {
        public VectorArray<int, char> Map { get; private set; }

        public Vector<int> Start { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct GuardState(Vector<int> Guard, Cardinal Direction, int Step, bool Exited, bool Loop);

        public GuardGallivant(string[] input)
        {
            this.Map = new(input, c => c);
            this.Start = this.Map.AxisEnumerator().First(x => x.Value == '^').Point;
            this.Map[this.Start] = '.';
        }

        public GuardGallivant(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        private int Patrol(Vector<int>? collision = null)
        {
            Queue<(Vector<int> Guard, Vector<int>? Collision, Cardinal Direction)> queue = new();
            HashSet<(Vector<int> Point, Cardinal Direction)> visited = [(this.Start.Clone(), Cardinal.North)];

            queue.Enqueue((this.Start.Clone(), collision, Cardinal.North));

            while (queue.Count != 0)
            {
                var state = queue.Dequeue();
                Vector<int> moved = state.Guard.Clone().Transform(state.Direction);

                if (!this.Map.Contains(moved))
                {
                    return collision == null ? Distinct(visited) : 0;
                }

                switch (collision == null || collision != moved ? this.Map[moved] : '#')
                {
                    case '#':
                        switch (state.Direction)
                        {
                            case Cardinal.North:
                                state.Direction = Cardinal.East;
                                break;
                            case Cardinal.South:
                                state.Direction = Cardinal.West;
                                break;
                            case Cardinal.East:
                                state.Direction = Cardinal.South;
                                break;
                            case Cardinal.West:
                                state.Direction = Cardinal.North;
                                break;
                        }

                        break;
                    case '.':
                        if (visited.Contains((moved, state.Direction)))
                        {
                            return 1;
                        }

                        visited.Add((moved.Clone(), state.Direction));
                        state.Guard = moved.Clone();
                        break;
                }

                queue.Enqueue((state.Guard, collision, state.Direction));
            }

            return 0;
        }

        private static int Distinct(HashSet<(Vector<int> Point, Cardinal Direction)> visited) => visited.Select(x => x.Point).Distinct().Count();

        public int WithoutCollisions() => this.Patrol();

        public int WithCollisions()
        {
            int result = 0;

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == '#'
                    || cell.Point == this.Start)
                {
                    continue;
                }

                if (this.Patrol(cell.Point) > 0)
                {
                    result++;
                }
            }

            return result;
        }

        public GuardGallivant RenderSilver(int renderEvery = 3)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<GuardState> states = this.TracePatrol();
            HashSet<Vector<int>> visited = [];
            List<string[]> frames = [];

            for (int i = 0; i < states.Count; i++)
            {
                GuardState state = states[i];
                visited.Add(state.Guard.Clone());

                if (i % renderEvery == 0 || i == states.Count - 1)
                {
                    frames.Add(this.BuildSilverFrame(
                        state.Guard,
                        state.Direction,
                        visited,
                        state.Exited
                            ? $"GUARD GALLIVANT // EXITED // VISITED {visited.Count:00000}"
                            : $"GUARD GALLIVANT // STEP {state.Step:00000} // VISITED {visited.Count:00000}"));
                }
            }

            for (int i = 0; i < 20; i++)
            {
                GuardState last = states.Last();

                frames.Add(this.BuildSilverFrame(
                    last.Guard,
                    last.Direction,
                    visited,
                    $"GUARD LEFT THE MAP // VISITED {visited.Count:00000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public GuardGallivant RenderGold(int renderEveryCandidates = 8, int renderEveryLoopStep = 2)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<Vector<int>> loopObstructions = [];
            List<string[]> frames = [];
            int tested = 0;

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == '#'
                    || cell.Point == this.Start)
                {
                    continue;
                }

                tested++;
                bool loops = this.Patrol(cell.Point) > 0;

                if (loops)
                {
                    loopObstructions.Add(cell.Point.Clone());
                }

                if (tested % renderEveryCandidates == 0 || loops)
                {
                    frames.Add(this.BuildGoldCandidateFrame(
                        cell.Point,
                        loopObstructions,
                        $"TESTING OBSTRUCTIONS // TESTED {tested:00000} // LOOPS {loopObstructions.Count:00000}"));
                }
            }

            Vector<int>? showcase = loopObstructions.FirstOrDefault();

            if (showcase != default)
            {
                List<GuardState> states = this.TracePatrol(showcase);
                Dictionary<Vector<int>, int> trail = [];

                for (int i = 0; i < states.Count; i++)
                {
                    GuardState state = states[i];
                    int mask = DirectionMask(state.Direction);

                    if (trail.TryGetValue(state.Guard, out int existing))
                    {
                        trail[state.Guard] = existing | mask;
                    }
                    else
                    {
                        trail[state.Guard.Clone()] = mask;
                    }

                    if (i % renderEveryLoopStep == 0 || i == states.Count - 1)
                    {
                        frames.Add(this.BuildGoldLoopFrame(
                            state.Guard,
                            state.Direction,
                            showcase,
                            loopObstructions,
                            trail,
                            state.Loop
                                ? $"LOOP FOUND // OBSTRUCTION OPTIONS {loopObstructions.Count:00000}"
                                : $"SHOWING LOOP // STEP {state.Step:00000} // OPTIONS {loopObstructions.Count:00000}"));
                    }
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildGoldCandidateFrame(
                    null,
                    loopObstructions,
                    $"GUARD LOOP OPTIONS // TOTAL {loopObstructions.Count:00000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<GuardState> TracePatrol(Vector<int>? collision = null)
        {
            List<GuardState> states = [];
            HashSet<(Vector<int> Point, Cardinal Direction)> visited = [(this.Start.Clone(), Cardinal.North)];

            Vector<int> guard = this.Start.Clone();
            Cardinal direction = Cardinal.North;
            int step = 0;

            states.Add(new(guard.Clone(), direction, step, Exited: false, Loop: false));

            while (true)
            {
                Vector<int> moved = guard.Clone().Transform(direction);

                if (!this.Map.Contains(moved))
                {
                    states.Add(new(guard.Clone(), direction, step, Exited: true, Loop: false));
                    return states;
                }

                char next = collision == null || collision != moved
                    ? this.Map[moved]
                    : '#';

                if (next == '#')
                {
                    direction = TurnRight(direction);
                    states.Add(new(guard.Clone(), direction, step, Exited: false, Loop: false));
                    continue;
                }

                if (next != '.')
                {
                    continue;
                }

                if (visited.Contains((moved, direction)))
                {
                    states.Add(new(moved.Clone(), direction, step + 1, Exited: false, Loop: true));
                    return states;
                }

                guard = moved.Clone();
                step++;
                visited.Add((guard.Clone(), direction));
                states.Add(new(guard.Clone(), direction, step, Exited: false, Loop: false));
            }
        }

        private string[] BuildSilverFrame(
            Vector<int> guard,
            Cardinal direction,
            HashSet<Vector<int>> visited,
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

                    if (point == guard)
                    {
                        sb.Append(ToGuard(direction));
                    }
                    else if (visited.Contains(point))
                    {
                        sb.Append('X');
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

        private string[] BuildGoldCandidateFrame(
            Vector<int>? currentCandidate,
            List<Vector<int>> loopObstructions,
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

                    if (point == this.Start)
                    {
                        sb.Append('^');
                    }
                    else if (currentCandidate != null && point == currentCandidate)
                    {
                        sb.Append('?');
                    }
                    else if (loopObstructions.Contains(point))
                    {
                        sb.Append('O');
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

        private string[] BuildGoldLoopFrame(
            Vector<int> guard,
            Cardinal direction,
            Vector<int> obstruction,
            List<Vector<int>> loopObstructions,
            Dictionary<Vector<int>, int> trail,
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

                    if (point == guard)
                    {
                        sb.Append(ToGuard(direction));
                    }
                    else if (point == obstruction)
                    {
                        sb.Append('O');
                    }
                    else if (trail.TryGetValue(point, out int mask))
                    {
                        sb.Append(ToTrail(mask));
                    }
                    else if (loopObstructions.Contains(point))
                    {
                        sb.Append('o');
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

        private static Cardinal TurnRight(Cardinal direction)
            => direction switch
            {
                Cardinal.North => Cardinal.East,
                Cardinal.East => Cardinal.South,
                Cardinal.South => Cardinal.West,
                Cardinal.West => Cardinal.North,
                _ => direction
            };

        private static char ToGuard(Cardinal direction)
            => direction switch
            {
                Cardinal.North => '^',
                Cardinal.East => '>',
                Cardinal.South => 'v',
                Cardinal.West => '<',
                _ => '^'
            };

        private static int DirectionMask(Cardinal direction)
            => direction switch
            {
                Cardinal.North or Cardinal.South => 1,
                Cardinal.East or Cardinal.West => 2,
                _ => 0
            };

        private static char ToTrail(int mask)
            => mask switch
            {
                1 => '|',
                2 => '-',
                3 => '+',
                _ => '.'
            };
    }
}
