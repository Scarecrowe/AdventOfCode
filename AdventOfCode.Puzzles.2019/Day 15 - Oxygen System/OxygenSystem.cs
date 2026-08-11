namespace AdventOfCode.Puzzles._2019.Day_15___Oxygen_System
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;
    using System.Text;

    public class OxygenSystem
    {
        public OxygenSystem(string program)
        {
            this.Cpu = new(program);
            this.Map = new()
            {
                { new(0, 0), Entity.Empty }
            };
            this.Droid = new(0, 0);
        }

        public OxygenSystem(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public int Distance { get; private set; }

        public int Minutes { get; private set; }

        private Vector<int> Droid { get; set; }

        private VectorDictionary<int, Entity> Map { get; }

        private IntcodeCpu Cpu { get; }

        public OxygenSystem BuildMap()
        {
            Queue<(Cardinal Direction, Vector<int> Droid, IntcodeCpu Cpu, int Distance)> queue = new();

            queue.Enqueue((Cardinal.North, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.South, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.West, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.East, new(0, 0), this.Cpu.Clone(), 0));

            while (queue.Count > 0)
            {
                (Cardinal Direction, Vector<int> Droid, IntcodeCpu Cpu, int Distance) state = queue.Dequeue();
                Vector<int> location = new(state.Droid);
                location += CardinalHelper.CardinalTransform<int>()[state.Direction];

                if (this.Map.ContainsKey(location))
                {
                    continue;
                }

                state.Cpu.Input.Enqueue(((int)state.Direction) + 1);
                state.Cpu.Run();
                Status value = (Status)state.Cpu.Output.Dequeue();

                if (value == Status.Moved)
                {
                    this.Map.Add(location, Entity.Empty);
                    queue.Enqueue((Cardinal.North, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.South, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.West, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.East, location, state.Cpu.Clone(), state.Distance + 1));
                }
                else if (value == Status.Found)
                {
                    this.Droid = new(location);
                    this.Distance = state.Distance + 1;
                }
                else
                {
                    this.Map.Add(location, Entity.Wall);
                }
            }

            return this;
        }

        public OxygenSystem RenderBuildMap(int renderEvery = 4)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            Queue<(Cardinal Direction, Vector<int> Droid, IntcodeCpu Cpu, int Distance)> queue = new();

            queue.Enqueue((Cardinal.North, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.South, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.West, new(0, 0), this.Cpu.Clone(), 0));
            queue.Enqueue((Cardinal.East, new(0, 0), this.Cpu.Clone(), 0));

            int step = 0;

            frames.Add(this.BuildFrame(new(0, 0), -1));

            while (queue.Count > 0)
            {
                (Cardinal Direction, Vector<int> Droid, IntcodeCpu Cpu, int Distance) state = queue.Dequeue();

                Vector<int> location = new(state.Droid);
                location += CardinalHelper.CardinalTransform<int>()[state.Direction];

                if (this.Map.ContainsKey(location))
                {
                    continue;
                }

                state.Cpu.Input.Enqueue(((int)state.Direction) + 1);
                state.Cpu.Run();

                Status value = (Status)state.Cpu.Output.Dequeue();

                if (value == Status.Moved)
                {
                    this.Map.Add(location, Entity.Empty);

                    queue.Enqueue((Cardinal.North, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.South, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.West, location, state.Cpu.Clone(), state.Distance + 1));
                    queue.Enqueue((Cardinal.East, location, state.Cpu.Clone(), state.Distance + 1));
                }
                else if (value == Status.Found)
                {
                    this.Droid = new(location);
                    this.Distance = state.Distance + 1;
                    this.Map.Add(location, Entity.Oxygen);
                }
                else
                {
                    this.Map.Add(location, Entity.Wall);
                }

                step++;

                if (step % renderEvery == 0 || value == Status.Found)
                {
                    frames.Add(this.BuildFrame(location, -1));
                }
            }

            for (int i = 0; i < 12; i++)
            {
                frames.Add(this.BuildFrame(this.Droid, -1));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public OxygenSystem PrintMap()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            Dictionary<Entity, string> display = new()
            {
                { Entity.Empty, "." },
                { Entity.Wall, "#" },
                { Entity.Oxygen, "O" }
            };

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Map.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(display[this.Map[new(x, y)]]);
                    }
                    else
                    {
                        PuzzleConsole.Write("#");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            return this;
        }

        public OxygenSystem FillMap()
        {
            this.Map[this.Droid] = Entity.Oxygen;

            int emptyCount = this.Map.Count(c => c.Value == Entity.Empty);
            List<Vector<int>> points = new() { this.Droid };
            List<Vector<int>> newPoints = new();

            while (emptyCount > 0)
            {
                foreach (Vector<int> point in points)
                {
                    foreach (VectorCell<int, Entity> adjacent in this.Map.AdjacentCardinal(point))
                    {
                        if (adjacent.Value == Entity.Empty)
                        {
                            this.Map[adjacent.Point] = Entity.Oxygen;
                            newPoints.Add(new(adjacent.Point));
                            emptyCount--;
                        }
                    }
                }

                points = newPoints;
                newPoints = new();

                this.Minutes++;
            }

            return this;
        }

        public OxygenSystem RenderFillMap()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            if (this.Map.Count <= 1 || this.Distance == 0)
            {
                this.BuildMap();
            }

            List<string[]> frames = [];

            this.Map[this.Droid] = Entity.Oxygen;

            int emptyCount = this.Map.Count(c => c.Value == Entity.Empty);

            List<Vector<int>> points = new() { this.Droid };
            List<Vector<int>> newPoints = new();

            this.Minutes = 0;

            frames.Add(this.BuildFrame(null, this.Minutes));

            while (emptyCount > 0)
            {
                foreach (Vector<int> point in points)
                {
                    foreach (VectorCell<int, Entity> adjacent in this.Map.AdjacentCardinal(point))
                    {
                        if (adjacent.Value == Entity.Empty)
                        {
                            this.Map[adjacent.Point] = Entity.Oxygen;
                            newPoints.Add(new(adjacent.Point));
                            emptyCount--;
                        }
                    }
                }

                points = newPoints;
                newPoints = new();

                this.Minutes++;

                frames.Add(this.BuildFrame(null, this.Minutes));
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(null, this.Minutes));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private void RenderFrame(Vector<int>? droid = null, int minute = -1)
        {
            if (this.Renderer == null || this.Map.Count == 0)
            {
                return;
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFrame(droid, minute)));
        }

        private string[] BuildFrame(Vector<int>? droid, int minute)
        {
            int padding = 2;

            int minY = this.Map.Min(c => c.Key.Y) - padding;
            int maxY = this.Map.Max(c => c.Key.Y) + padding;
            int minX = this.Map.Min(c => c.Key.X) - padding;
            int maxX = this.Map.Max(c => c.Key.X) + padding;

            List<string> result = [];

            if (minute >= 0)
            {
                result.Add($"OXYGEN FILL // MINUTE {minute:000}");
            }
            else
            {
                result.Add($"DROID EXPLORATION // DISTANCE {this.Distance:000}");
            }

            result.Add(string.Empty);

            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new(x, y);

                    if (droid != null && point == droid)
                    {
                        sb.Append('@');
                    }
                    else if (point == new Vector<int>(0, 0))
                    {
                        sb.Append('+');
                    }
                    else if (this.Map.TryGetValue(point, out Entity entity))
                    {
                        sb.Append(entity switch
                        {
                            Entity.Empty => '.',
                            Entity.Wall => '#',
                            Entity.Oxygen => '~',
                            _ => '?'
                        });
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames
                .SelectMany(frame => frame)
                .Max(row => row.Length);

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
