namespace AdventOfCode.Puzzles._2022.Day_23___Unstable_Diffusion
{
    using AdventOfCode.Core;

    public class UnstableDiffusion
    {
        public UnstableDiffusion(string[] input)
        {
            this.Map = new();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[0].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        this.Map.Add(new(x, y), 1);
                    }
                }
            }
        }

        public VectorDictionary<int, int> Map { get; private set; }

        public int Round { get; private set; }

        public UnstableDiffusion PrintMap()
        {
            Vector<int> min = new(this.Map.Min(x => x.Key.X), this.Map.Min(x => x.Key.Y));
            Vector<int> max = new(this.Map.Max(x => x.Key.X), this.Map.Max(x => x.Key.Y));

            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    if (!this.Map.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(".");
                    }
                    else
                    {
                        PuzzleConsole.Write("#");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();

            return this;
        }

        public int EmptyGround()
        {
            Vector<int> min = new(this.Map.Min(x => x.Key.X), this.Map.Min(x => x.Key.Y));
            Vector<int> max = new(this.Map.Max(x => x.Key.X), this.Map.Max(x => x.Key.Y));

            int result = 0;

            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    if (!this.Map.ContainsKey(new(x, y)))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public UnstableDiffusion Run(bool infinite = false)
        {
            Dictionary<Vector<int>, List<Vector<int>>> moves = new();
            List<Cardinal> directions = CardinalHelper.CardinalTransform<int>().Select(x => x.Key).ToList();
            this.Round = 1;

            while (true)
            {
                foreach (var elf in this.Map)
                {
                    List<VectorCell<int, int>> adjacent = this.Map.AdjacentInterCardinal(elf.Key).ToList();

                    if (adjacent.Any())
                    {
                        foreach (Cardinal cardinal in directions)
                        {
                            if (!HasGroupMove(cardinal, adjacent))
                            {
                                continue;
                            }

                            VectorCell<int, int>? cell = CardinalHelper.AllCells<int, int>().FirstOrDefault(x => x.Direction == cardinal);

                            if (moves.ContainsKey((cell?.Point ?? new(0, 0)) + elf.Key))
                            {
                                moves[(cell?.Point ?? new(0, 0)) + elf.Key].Add(elf.Key);
                            }
                            else
                            {
                                moves.Add((cell?.Point ?? new(0, 0)) + elf.Key, new() { elf.Key });
                            }

                            break;
                        }
                    }
                }

                Cardinal first = directions.First();

                if (!moves.Any())
                {
                    break;
                }

                foreach (var move in moves)
                {
                    if (move.Value.Count == 1)
                    {
                        this.Map.Remove(move.Value.First());
                        this.Map.Add(move.Key, 1);
                    }
                }

                directions.Remove(first);
                directions.Add(first);
                moves.Clear();
                this.Round++;

                if (!infinite && this.Round > 10)
                {
                    break;
                }
            }

            return this;
        }

        private static bool HasGroupMove(Cardinal direction, List<VectorCell<int, int>> adjacent)
        {
            foreach (var check in CardinalHelper.CardinalGroupMap[direction])
            {
                if (adjacent.Any(x => x.Direction == check))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
