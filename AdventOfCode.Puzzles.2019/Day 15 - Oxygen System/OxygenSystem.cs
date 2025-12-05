namespace AdventOfCode.Puzzles._2019.Day_15___Oxygen_System
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

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
    }
}
