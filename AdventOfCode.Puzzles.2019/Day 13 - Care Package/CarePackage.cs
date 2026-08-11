namespace AdventOfCode.Puzzles._2019.Day_13___Care_Package
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;
    using System.Text;

    public class CarePackage
    {
        public CarePackage(string program)
        {
            this.Cpu = new(program);
            this.Display = new();
        }

        public CarePackage(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

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

        public CarePackage RenderGame(int quarters = 2, int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            this.Cpu.Memory.Write(0, quarters);
            this.Cpu.Run();
            this.GetDisplay();

            frames.Add(this.BuildFrame());

            if (quarters == 1)
            {
                this.RenderBoot(renderEvery);
                return this;
            }

            this.Cpu.Input.Enqueue((int)Joystick.Neutral);

            int step = 0;

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

                step++;

                if (step % renderEvery == 0)
                {
                    frames.Add(this.BuildFrame());
                }
            }

            frames.Add(this.BuildFrame());

            string[] finalFrame = this.BuildFrame();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(finalFrame);
            }

            this.RenderPaddedFrames(frames);

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

        private void RenderFrame()
        {
            if (this.Renderer == null || this.Display.Count == 0)
            {
                return;
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFrame()));
        }

        private string[] BuildFrame()
        {
            int minY = this.Display.Min(c => c.Key.Y);
            int maxY = this.Display.Max(c => c.Key.Y);
            int minX = this.Display.Min(c => c.Key.X);
            int maxX = this.Display.Max(c => c.Key.X);

            Dictionary<Tile, char> display = new()
            {
                { Tile.Empty, ' ' },
                { Tile.Ball, 'o' },
                { Tile.Wall, '#' },
                { Tile.Paddle, '=' },
                { Tile.Block, '▒' }
            };

            List<string> result = [];

            result.Add($"SCORE: {this.Score}   BLOCKS: {this.CountBlocks()}");
            result.Add(new string('-', (maxX - minX) + 1));

            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new(x, y);

                    if (this.Display.TryGetValue(point, out Tile tile))
                    {
                        sb.Append(display[tile]);
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

        private string[] BuildFrame(Dictionary<Vector<int>, Tile> display)
        {
            Dictionary<Tile, char> chars = new()
            {
                { Tile.Empty, ' ' },
                { Tile.Ball, 'o' },
                { Tile.Wall, '#' },
                { Tile.Paddle, '=' },
                { Tile.Block, '▒' }
            };

            int minY = this.Display.Min(c => c.Key.Y);
            int maxY = this.Display.Max(c => c.Key.Y);
            int minX = this.Display.Min(c => c.Key.X);
            int maxX = this.Display.Max(c => c.Key.X);

            List<string> result = [];

            result.Add($"SCORE: {this.Score}   BLOCKS: {display.Count(c => c.Value == Tile.Block)}");
            result.Add(new string('-', (maxX - minX) + 1));

            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new(x, y);

                    if (display.TryGetValue(point, out Tile tile))
                    {
                        sb.Append(chars[tile]);
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

        private void RenderBoot(int renderEvery = 1)
        {
            List<string[]> frames = [];

            Dictionary<Vector<int>, Tile> bootDisplay = new();

            List<KeyValuePair<Vector<int>, Tile>> tiles = this.Display
                .OrderBy(x => x.Key.Y)
                .ThenBy(x => x.Key.X)
                .ToList();

            int step = 0;

            foreach (KeyValuePair<Vector<int>, Tile> tile in tiles)
            {
                if (tile.Value == Tile.Empty)
                {
                    continue;
                }

                bootDisplay[tile.Key] = tile.Value;

                step++;

                if (step % renderEvery == 0)
                {
                    frames.Add(this.BuildFrame(bootDisplay));
                }
            }

            string[] finalFrame = this.BuildFrame(bootDisplay);

            frames.Add(finalFrame);

            this.RenderPaddedFrames(frames);
        }
    }
}
