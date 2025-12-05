namespace AdventOfCode.Puzzles._2019.Day_13___Care_Package
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class CarePackage
    {
        public CarePackage(string program)
        {
            this.Cpu = new(program);
            this.Display = new();
        }

        public IntcodeCpu Cpu { get; }

        public VectorDictionary<int, Tile> Display { get; }

        public long Score { get; private set; }

        public CarePackage Play(int quaters = 1)
        {
            this.Cpu.Memory.Write(0, quaters);
            this.Cpu.Run();
            this.GetDisplay();

            if (quaters == 1)
            {
                return this;
            }

            this.Cpu.Input.Enqueue((int)Joystick.Neutral);

            while (this.Cpu.State != IntcodeCpuState.Terminated)
            {
                this.Cpu.Run();
                this.GetDisplay();

                Vector<int> ball = this.GetBall();
                Vector<int> paddle = this.GetPaddle();

                if (paddle.X < ball.X)
                {
                    this.Cpu.Input.Enqueue((int)Joystick.Right);
                }
                else if (paddle.X > ball.X)
                {
                    this.Cpu.Input.Enqueue((int)Joystick.Left);
                }
                else
                {
                    this.Cpu.Input.Enqueue((int)Joystick.Neutral);
                }
            }

            return this;
        }

        public string Print()
        {
            long minY = this.Display.Min(c => c.Key.Y);
            long maxY = this.Display.Max(c => c.Key.Y);
            long minX = this.Display.Min(c => c.Key.X);
            long maxX = this.Display.Max(c => c.Key.X);

            Dictionary<Tile, string> display = new()
            {
                { Tile.Empty, " " },
                { Tile.Ball, "." },
                { Tile.Wall, "#" },
                { Tile.Paddle, "-" },
                { Tile.Block, "^" }
            };

            PuzzleConsole.WriteLine($"SCORE: {this.Score}");

            PuzzleConsole.Write("  ");

            for (long x = minX; x <= maxX; x++)
            {
                if (x >= 10)
                {
                    PuzzleConsole.Write($"{x.ToString().Substring(0, 1)}");
                }
                else
                {
                    PuzzleConsole.Write(" ");
                }
            }

            PuzzleConsole.WriteLine();

            PuzzleConsole.Write("  ");

            for (long x = minX; x <= maxX; x++)
            {
                if (x < 10)
                {
                    PuzzleConsole.Write($"{x}");
                }
                else
                {
                    PuzzleConsole.Write($"{x.ToString().Substring(1, 1)}");
                }
            }

            PuzzleConsole.WriteLine();

            for (long y = minY; y <= maxY; y++)
            {
                PuzzleConsole.Write($"{y,2}");

                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Display.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(display[this.Display[new(x, y)]]);
                    }
                    else
                    {
                        PuzzleConsole.Write(" ");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.Write("  ");

            for (long x = minX; x <= maxX; x++)
            {
                if (x < 10)
                {
                    PuzzleConsole.Write($"{x}");
                }
                else
                {
                    PuzzleConsole.Write($"{x.ToString().Substring(1, 1)}");
                }
            }

            PuzzleConsole.WriteLine();

            PuzzleConsole.Write("  ");

            for (long x = minX; x <= maxX; x++)
            {
                if (x >= 10)
                {
                    PuzzleConsole.Write($"{x.ToString().Substring(0, 1)}");
                }
                else
                {
                    PuzzleConsole.Write(" ");
                }
            }

            PuzzleConsole.WriteLine();

            return string.Empty;
        }

        public int CountBlocks() => this.Display.Count(c => c.Value == Tile.Block);

        private Vector<int> GetPaddle() => this.Display.FirstOrDefault(c => c.Value == Tile.Paddle).Key;

        private Vector<int> GetBall() => this.Display.FirstOrDefault(c => c.Value == Tile.Ball).Key;

        private void GetDisplay()
        {
            List<long> values = new();
            int count = 0;

            while (this.Cpu.Output.Any())
            {
                values.Add(this.Cpu.Output.Dequeue());

                count++;

                if (count == 3)
                {
                    Vector<int> key = new(values[0], values[1]);

                    if (key.X == -1 && key.Y == 0)
                    {
                        this.Score = values[2];
                        values.Clear();
                        count = 0;
                        continue;
                    }

                    if (this.Display.ContainsKey(key))
                    {
                        this.Display[key] = (Tile)values[2];
                    }
                    else
                    {
                        this.Display.Add(key, (Tile)values[2]);
                    }

                    values.Clear();
                    count = 0;
                }
            }
        }
    }
}
