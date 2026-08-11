namespace AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Terraria;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ReservoirResearch : ITerrariaRenderer
    {
        public ReservoirResearch(string[] input)
        {
            this.ClayMin = new(0, 0);
            this.ClayMax = new(0, 0);

            List<(Vector<long> Min, Vector<long> Max)> clay = Parse(input);

            this.ClayMax.X = clay.Max(c => c.Max.X) + 1;
            this.ClayMax.Y = clay.Max(c => c.Max.Y) + 1;
            this.ClayMin.X = clay.Min(c => c.Min.X) - 1;
            this.ClayMin.Y = clay.Min(c => c.Min.Y) - 1;

            this.Map = new((this.ClayMax.X - this.ClayMin.X) + 1, (this.ClayMax.Y - this.ClayMin.Y) + 1);

            foreach ((Vector<long> min, Vector<long> max) in clay)
            {
                for (long y = min.Y; y <= max.Y; y++)
                {
                    for (long x = min.X; x <= max.X; x++)
                    {
                        this.Map[new Vector<long>(x, y) - this.ClayMin] = EntityType.Clay;
                    }
                }
            }

            this.Queue = new();
        }

        public ReservoirResearch(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public Vector<long> ClayMin { get; }

        public Vector<long> ClayMax { get; }

        public VectorArray<long, EntityType> Map { get; private set; }

        private Queue<Stream> Queue { get; set; }

        private WaterFallRenderer WaterFall { get; set; }

        private IFrameRenderer? Renderer { get; }

        public long Settle(bool countWater = true)
        {
            Queue<Stream> queue = new();

            queue.Enqueue(new(new(500 - this.ClayMin.X, 1)));

            Stream? stream;

            while (queue.Count > 0)
            {
                stream = queue.Dequeue();

                if (!this.Map.IsVectorInRange(stream.Point.X, stream.Point.Y))
                {
                    continue;
                }

                stream.Move(this.Map, ref queue);
            }

            return this.WaterCount(countWater);
        }

        public long WaterCount(bool countWater = true)
        {
            long count = 0;
            for (int y = 1; y < this.ClayMax.Y - this.ClayMin.Y; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if ((countWater && this.Map[y, x] == EntityType.Water) || this.Map[y, x] == EntityType.Settled)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public Vector<long> Animate()
        {
            if (!this.Queue.Any())
            {
                this.Queue.Enqueue(new(new(500 - this.ClayMin.X, 1)));
            }

            Stream? stream;

            while (this.Queue.Count > 0)
            {
                stream = this.Queue.Dequeue();

                if (!this.Map.IsVectorInRange(stream.Point.X, stream.Point.Y))
                {
                    continue;
                }

                var queue = this.Queue;
                stream.Move(this.Map, ref queue);

                return stream.Point;
            }

            return new Vector<long>(-1, -1);
        }

        public ReservoirResearch RenderSilver(int renderEvery = 1)
        {
            return this.RenderWater(countWater: true, renderEvery: renderEvery);
        }

        public ReservoirResearch RenderGold(int renderEvery = 1)
        {
            return this.RenderWater(countWater: false, renderEvery: renderEvery);
        }

        private ReservoirResearch RenderWater(bool countWater, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            Vector<long> furthest = new(500 - this.ClayMin.X, 1);
            long furthestDistance = 0;
            long step = 0;

            frames.Add(this.BuildFrame(
                furthest,
                $"RESERVOIR RESEARCH // SCAN {this.Map.Width:0000}x{this.Map.Height:0000} // WATER {0:00000}"));

            while (true)
            {
                Vector<long> current = this.Animate();

                if (current == new Vector<long>(-1, -1))
                {
                    break;
                }

                step++;

                long distance = this.DistanceFromSpring(current);

                if (distance >= furthestDistance)
                {
                    furthest = current;
                    furthestDistance = distance;
                }

                if (step % renderEvery == 0)
                {
                    frames.Add(this.BuildFrame(
                        furthest,
                        countWater
                            ? $"RESERVOIR RESEARCH // STREAM {step:00000} // REACHED {this.WaterCount(true):00000}"
                            : $"RESERVOIR RESEARCH // STREAM {step:00000} // RETAINED {this.WaterCount(false):00000}"));
                }
            }

            frames.Add(this.BuildFrame(
                furthest,
                countWater
                    ? $"WATER REACHED {this.WaterCount(true):00000} // FURTHEST STREAM MARKED"
                    : $"WATER RETAINED {this.WaterCount(false):00000} // FURTHEST STREAM MARKED"));

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    furthest,
                    countWater
                        ? $"WATER REACHED {this.WaterCount(true):00000} // FURTHEST STREAM MARKED"
                        : $"WATER RETAINED {this.WaterCount(false):00000} // FURTHEST STREAM MARKED"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private long DistanceFromSpring(Vector<long> point)
        {
            long springX = 500 - this.ClayMin.X;

            return Math.Abs(point.X - springX) + point.Y;
        }

        private string[] BuildFrame(Vector<long> marker, string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<long> point = new(x, y);

                    if (point == marker)
                    {
                        sb.Append('@');
                        continue;
                    }

                    if (point == new Vector<long>(500 - this.ClayMin.X, 0))
                    {
                        sb.Append('+');
                        continue;
                    }

                    switch (this.Map[y, x])
                    {
                        case EntityType.Water:
                            sb.Append('|');
                            break;
                        case EntityType.Air:
                            sb.Append('.');
                            break;
                        case EntityType.Clay:
                            sb.Append('#');
                            break;
                        case EntityType.Settled:
                            sb.Append('~');
                            break;
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

        private static List<(Vector<long> Min, Vector<long> Max)> Parse(string[] input)
            => input.Select(x => ParseClay(x)).ToList();

        private static (Vector<long> Min, Vector<long> Max) ParseClay(string line)
        {
            Vector<long> min = new(0, 0);
            Vector<long> max = new(0, 0);
            string[] tokens = line.Split(", ");
            string[] axis = tokens[0].Split("=");
            int[] range = axis[1].Split("..").ToInt();

            if (axis[0] == "x")
            {
                min.X = range[0];

                if (range.Length == 1)
                {
                    max.X = min.X;
                }
                else
                {
                    max.X = range[1];
                }
            }
            else
            {
                min.Y = range[0];

                if (range.Length == 1)
                {
                    max.Y = min.Y;
                }
                else
                {
                    max.Y = range[1];
                }
            }

            axis = tokens[1].Split("=");
            range = axis[1].Split("..").ToInt();

            if (axis[0] == "x")
            {
                min.X = range[0];

                if (range.Length == 1)
                {
                    max.X = min.X;
                }
                else
                {
                    max.X = range[1];
                }
            }
            else
            {
                min.Y = range[0];

                if (range.Length == 1)
                {
                    max.Y = min.Y;
                }
                else
                {
                    max.Y = range[1];
                }
            }

            return (min, max);
        }

        private void Print((long x, long y) point)
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (point == (x, y))
                    {
                        sb.Append('[');
                    }

                    switch (this.Map[y, x])
                    {
                        case EntityType.Water:
                            sb.Append('|');
                            break;
                        case EntityType.Air:
                            sb.Append('.');
                            break;
                        case EntityType.Clay:
                            sb.Append('#');
                            break;
                        case EntityType.Settled:
                            sb.Append('~');
                            break;
                    }

                    if (point == (x, y))
                    {
                        sb.Append(']');
                    }
                }

                PuzzleConsole.WriteLine(sb.ToString());
            }

            PuzzleConsole.WriteLine();
        }

        public void Animate2D()
        {
            Console.Clear();
            this.WaterFall = new WaterFallRenderer();
            this.WaterFall.Render();
        }
    }
}
