namespace AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research
{
    using System.Text;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ReservoirResearch
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

        public Vector<long> ClayMin { get; }

        public Vector<long> ClayMax { get; }

        public VectorArray<long, EntityType> Map { get; private set; }

        private Queue<Stream> Queue { get; set; }

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
    }
}
