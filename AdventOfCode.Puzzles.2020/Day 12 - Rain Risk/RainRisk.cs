namespace AdventOfCode.Puzzles._2020.Day_12___Rain_Risk
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class RainRisk
    {
        public RainRisk(string[] input)
        {
            this.Input = input;
            this.Direction = 1;
            this.Point = new(0, 0);
            this.WayPoint = new(0, 0);
        }

        public RainRisk(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private int[] Directions { get; } = ['N', 'E', 'S', 'W'];

        private Vector<int> Point { get; set; }

        private Vector<int> WayPoint { get; set; }

        private uint Direction { get; set; }

        private string[] Input { get; }

        private readonly record struct RenderState(
            Vector<int> Ship,
            Vector<int> WayPoint,
            char Facing,
            string Instruction,
            int Step,
            long Distance);

        public long Distance()
        {
            this.Point = new(0, 0);
            this.Direction = 1;
            this.Input.ForEach(this.Process);

            return this.Point.Absolute();
        }

        public long DistanceWithWaypoint()
        {
            this.Point = new(0, 0);
            this.WayPoint = new(10, 1);
            this.Direction = 1;
            this.Input.ForEach(this.ProcessWithWaypoint);

            return this.Point.Absolute();
        }

        public RainRisk RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<RenderState> states = this.BuildSilverStates();
            this.RenderStates(states, waypointMode: false, renderEvery);

            return this;
        }

        public RainRisk RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<RenderState> states = this.BuildGoldStates();
            this.RenderStates(states, waypointMode: true, renderEvery);

            return this;
        }

        private List<RenderState> BuildSilverStates()
        {
            this.Point = new(0, 0);
            this.Direction = 1;

            List<RenderState> states = [this.CreateState("START", 0)];

            for (int i = 0; i < this.Input.Length; i++)
            {
                this.Process(this.Input[i]);
                states.Add(this.CreateState(this.Input[i], i + 1));
            }

            return states;
        }

        private List<RenderState> BuildGoldStates()
        {
            this.Point = new(0, 0);
            this.WayPoint = new(10, 1);
            this.Direction = 1;

            List<RenderState> states = [this.CreateState("START", 0)];

            for (int i = 0; i < this.Input.Length; i++)
            {
                this.ProcessWithWaypoint(this.Input[i]);
                states.Add(this.CreateState(this.Input[i], i + 1));
            }

            return states;
        }

        private RenderState CreateState(string instruction, int step)
            => new(
                this.Point,
                this.WayPoint,
                (char)this.Directions[this.Direction],
                instruction,
                step,
                this.Point.Absolute());

        private void RenderStates(List<RenderState> states, bool waypointMode, int renderEvery)
        {
            const int viewWidth = 96;
            const int viewHeight = 32;

            List<string[]> frames = [];
            HashSet<Vector<int>> trail = [];

            for (int i = 0; i < states.Count; i++)
            {
                RenderState state = states[i];
                trail.Add(state.Ship);

                if (i % renderEvery == 0 || i == states.Count - 1)
                {
                    Bounds bounds = waypointMode
                        ? Bounds.Around(state.Ship, 600, 300)
                        : Bounds.Around(state.Ship, 300, 160);

                    frames.Add(this.BuildFrame(
                        state,
                        trail,
                        bounds,
                        viewWidth,
                        viewHeight,
                        waypointMode));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                RenderState state = states.Last();

                Bounds bounds = waypointMode
                    ? Bounds.Around(state.Ship, 600, 300)
                    : Bounds.Around(state.Ship, 300, 160);

                frames.Add(this.BuildFrame(
                    state,
                    trail,
                    bounds,
                    viewWidth,
                    viewHeight,
                    waypointMode));
            }

            this.RenderPaddedFrames(frames);
        }

        private string[] BuildFrame(
            RenderState state,
            HashSet<Vector<int>> trail,
            Bounds bounds,
            int width,
            int height,
            bool waypointMode)
        {
            char[,] canvas = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    canvas[y, x] = ' ';
                }
            }

            Vector<int> origin = bounds.Project(new(0, 0), width, height);

            for (int x = 0; x < width; x++)
            {
                canvas[origin.Y, x] = '-';
            }

            for (int y = 0; y < height; y++)
            {
                canvas[y, origin.X] = '|';
            }

            canvas[origin.Y, origin.X] = '+';

            foreach (Vector<int> point in trail)
            {
                if (!bounds.Contains(point))
                {
                    continue;
                }

                Vector<int> projected = bounds.Project(point, width, height);
                canvas[projected.Y, projected.X] = '~';
            }

            Vector<int> ship = bounds.Project(state.Ship, width, height);
            canvas[ship.Y, ship.X] = '@';

            if (waypointMode)
            {
                Vector<int> waypointAbsolute = state.Ship + state.WayPoint;
                Vector<int> waypoint = bounds.Project(waypointAbsolute, width, height);

                this.DrawLine(canvas, ship, waypoint, '*');
                canvas[ship.Y, ship.X] = '@';
                canvas[waypoint.Y, waypoint.X] = 'W';
            }

            List<string> result = [];

            result.Add(
                waypointMode
                    ? $"RAIN RISK // WAYPOINT // STEP {state.Step:0000} // {state.Instruction,-5} // DISTANCE {state.Distance:000000}"
                    : $"RAIN RISK // HEADING {state.Facing} // STEP {state.Step:0000} // {state.Instruction,-5} // DISTANCE {state.Distance:000000}");

            result.Add(string.Empty);

            for (int y = 0; y < height; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < width; x++)
                {
                    row.Append(canvas[y, x]);
                }

                result.Add(row.ToString());
            }

            return [.. result];
        }

        private void DrawLine(char[,] canvas, Vector<int> start, Vector<int> end, char value)
        {
            int width = canvas.GetLength(1);
            int height = canvas.GetLength(0);

            int dx = Math.Abs(end.X - start.X);
            int dy = -Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int error = dx + dy;

            int x = start.X;
            int y = start.Y;

            while (true)
            {
                if (x >= 0 && y >= 0 && x < width && y < height && canvas[y, x] == ' ')
                {
                    canvas[y, x] = value;
                }

                if (x == end.X && y == end.Y)
                {
                    break;
                }

                int e2 = 2 * error;

                if (e2 >= dy)
                {
                    error += dy;
                    x += sx;
                }

                if (e2 <= dx)
                {
                    error += dx;
                    y += sy;
                }
            }
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

        private void Process(string instruction)
        {
            char mode = instruction[0];
            int value = instruction[1..].ToInt();

            switch (mode)
            {
                case 'N': this.Point.Y += value; break;
                case 'S': this.Point.Y -= value; break;
                case 'E': this.Point.X += value; break;
                case 'W': this.Point.X -= value; break;
                case 'L': this.Direction = (this.Direction + 4 - ((uint)value / 90)) % 4; break;
                case 'R': this.Direction = (this.Direction + ((uint)value / 90)) % 4; break;
                case 'F': this.Process($"{(char)this.Directions[this.Direction]}{value}"); break;
            }
        }

        private void ProcessWithWaypoint(string instruction)
        {
            char mode = instruction[0];
            int value = instruction[1..].ToInt();

            switch (mode)
            {
                case 'N': this.WayPoint.Y += value; break;
                case 'S': this.WayPoint.Y -= value; break;
                case 'E': this.WayPoint.X += value; break;
                case 'W': this.WayPoint.X -= value; break;
                case 'L': this.WayPoint = this.WayPoint.Rotate(value); break;
                case 'R': this.WayPoint = this.WayPoint.Rotate(-value); break;
                case 'F': this.Point += this.WayPoint * value; break;
            }
        }

        private readonly record struct Bounds(int MinX, int MaxX, int MinY, int MaxY)
        {
            public bool Contains(Vector<int> point)
                => point.X >= this.MinX
                && point.X <= this.MaxX
                && point.Y >= this.MinY
                && point.Y <= this.MaxY;

            public static Bounds Around(Vector<int> centre, int width, int height)
            {
                int halfWidth = width / 2;
                int halfHeight = height / 2;

                return new(
                    centre.X - halfWidth,
                    centre.X + halfWidth,
                    centre.Y - halfHeight,
                    centre.Y + halfHeight);
            }

            public static Bounds From(List<RenderState> states, bool waypointMode)
            {
                List<Vector<int>> points = states.Select(x => x.Ship).ToList();

                if (waypointMode)
                {
                    points.AddRange(states.Select(x => x.Ship + x.WayPoint));
                }

                points.Add(new(0, 0));

                return new(
                    points.Min(p => p.X),
                    points.Max(p => p.X),
                    points.Min(p => p.Y),
                    points.Max(p => p.Y));
            }

            public Vector<int> Project(Vector<int> point, int width, int height)
            {
                double xRange = Math.Max(1, this.MaxX - this.MinX);
                double yRange = Math.Max(1, this.MaxY - this.MinY);

                int x = (int)Math.Round((point.X - this.MinX) / xRange * (width - 1));
                int y = height - 1 - (int)Math.Round((point.Y - this.MinY) / yRange * (height - 1));

                return new(
                    Math.Clamp(x, 0, width - 1),
                    Math.Clamp(y, 0, height - 1));
            }
        }
    }
}