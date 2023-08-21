namespace AdventOfCode.Puzzles._2019.Day_19___Tractor_Beam
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class TractorBeam
    {
        public TractorBeam(string program)
        {
            this.Cpu = new(program);
            this.Map = new();
        }

        public IntcodeCpu Cpu { get; }

        public int TractorBeamArea => this.Map.Count(c => c.Value == Entity.Pulled);

        private VectorDictionary<int, Entity> Map { get; }

        public TractorBeam BuildMap(int size, int min, int max)
        {
            for (int y = min; y < max; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    this.Cpu.Reset();
                    this.Cpu.Input.Enqueue(x);
                    this.Cpu.Input.Enqueue(y);
                    this.Cpu.Run();
                    this.Map.Add(new(x, y), (Entity)this.Cpu.Output.Dequeue());
                }
            }

            return this;
        }

        public TractorBeam PrintMap()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            Dictionary<Entity, string> display = new()
            {
                { Entity.Stationary, "." },
                { Entity.Pulled, "#" }
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

        public long ClosestPoint()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            int highest = 0;

            for (long y = minY; y <= maxY; y++)
            {
                int count = this.Map.Count(c => c.Key.Y == y && c.Value == Entity.Pulled);

                if (count >= 100)
                {
                    long xFirst = this.Map.FirstOrDefault(c => c.Key.Y == y && c.Value == Entity.Pulled).Key.X;
                    long xLast = this.Map.LastOrDefault(c => c.Key.Y == y && c.Value == Entity.Pulled).Key.X;

                    for (long x = xFirst; x <= xLast; x++)
                    {
                        count = 0;

                        for (long yy = y; yy <= y + 100; yy++)
                        {
                            if (this.Map.ContainsKey(new(x, yy)))
                            {
                                count += (int)this.Map[new(x, yy)];
                            }
                        }

                        if (count > highest)
                        {
                            highest = count;
                        }

                        if (count == 100)
                        {
                            if (this.Map[new(x + 99, y)] == Entity.Pulled && this.Map[new(x, y + 99)] == Entity.Pulled)
                            {
                                return (x * 10000) + y;
                            }
                        }
                    }
                }
            }

            return -1;
        }
    }
}
