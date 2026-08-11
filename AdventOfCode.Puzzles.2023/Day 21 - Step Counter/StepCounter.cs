namespace AdventOfCode.Puzzles._2023.Day_21___Step_Counter
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class StepCounter
    {
        private static readonly int[] Dr = { -1, 1, 0, 0 };
        private static readonly int[] Dc = { 0, 0, -1, 1 };

        public string[] Map { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public (int R, int C) Start { get; set; }

        public StepCounter(string[] input)
        {
            this.Map = input;
            this.Rows = input.Length;
            this.Columns = input[0].Length;
            this.Start = FindStart('S', input);
        }

        public StepCounter(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public int ShortWalk()
        {
            int steps = 64;

            HashSet<(int r, int c)> frontier = [(this.Start.R, this.Start.C)];

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int s = 0; s < steps; s++)
            {
                HashSet<(int r, int c)> next = new();

                foreach (var (r, d) in frontier)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int nr = r + dr[i];
                        int nc = d + dc[i];

                        if (nr < 0 || nr >= this.Rows || nc < 0 || nc >= this.Columns)
                        {
                            continue;
                        }

                        if (this.Map[nr][nc] != '#')
                        {
                            next.Add((nr, nc));
                        }
                    }
                }

                frontier = next;
            }

            return frontier.Count;
        }

        public long LongWalk()
        {
            int baseOffset = this.Rows / 2;
            long N = 26501365;

            long k = (N - baseOffset) / this.Rows;

            int s0 = baseOffset;
            int s1 = baseOffset + this.Rows;
            int s2 = baseOffset + 2 * this.Rows;

            long f0 = CountReach(this.Map, this.Rows, this.Columns, this.Start.R, this.Start.C, s0);
            long f1 = CountReach(this.Map, this.Rows, this.Columns, this.Start.R, this.Start.C, s1);
            long f2 = CountReach(this.Map, this.Rows, this.Columns, this.Start.R, this.Start.C, s2);

            long a = (f2 - 2 * f1 + f0) / 2;
            long b = f1 - f0 - a;
            long c = f0;

            return a * k * k + b * k + c;

        }

        public StepCounter RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<(int r, int c)> frontier = [(this.Start.R, this.Start.C)];
            HashSet<(int r, int c)> everReached = [(this.Start.R, this.Start.C)];

            frames.Add(this.BuildFixedFrame(
                frontier,
                everReached,
                0,
                64,
                "STEP COUNTER // FINITE FARM"));

            for (int step = 1; step <= 64; step++)
            {
                frontier = this.WalkFinite(frontier);

                foreach ((int r, int c) point in frontier)
                {
                    everReached.Add(point);
                }

                if (step % renderEvery == 0 || step == 64)
                {
                    frames.Add(this.BuildFixedFrame(
                        frontier,
                        everReached,
                        step,
                        64,
                        "STEP COUNTER // FINITE FARM"));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFixedFrame(
                    frontier,
                    everReached,
                    64,
                    64,
                    $"64 STEPS REACHED // PLOTS {frontier.Count:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public StepCounter RenderGold(int previewSteps = 327, int renderEvery = 4, int radius = 38)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<(int r, int c)> frontier = [(this.Start.R, this.Start.C)];
            HashSet<(int r, int c)> everReached = [(this.Start.R, this.Start.C)];

            long targetSteps = 26501365;
            int baseOffset = this.Rows / 2;
            long k = (targetSteps - baseOffset) / this.Rows;

            frames.Add(this.BuildInfiniteWindowFrame(
                frontier,
                everReached,
                0,
                previewSteps,
                radius,
                $"INFINITE FARM // TARGET {targetSteps} // K {k}"));

            for (int step = 1; step <= previewSteps; step++)
            {
                frontier = this.WalkInfinite(frontier);

                foreach ((int r, int c) point in frontier)
                {
                    if (Math.Abs(point.r - this.Start.R) <= radius && Math.Abs(point.c - this.Start.C) <= radius)
                    {
                        everReached.Add(point);
                    }
                }

                if (step % renderEvery == 0 || step == previewSteps)
                {
                    frames.Add(this.BuildInfiniteWindowFrame(
                        frontier,
                        everReached,
                        step,
                        previewSteps,
                        radius,
                        $"INFINITE FARM // TARGET {targetSteps} // K {k}"));
                }
            }

            long answer = this.LongWalk();

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildInfiniteWindowFrame(
                    frontier,
                    everReached,
                    previewSteps,
                    previewSteps,
                    radius,
                    $"QUADRATIC EXTRAPOLATION // ANSWER {answer}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private HashSet<(int r, int c)> WalkFinite(HashSet<(int r, int c)> frontier)
        {
            HashSet<(int r, int c)> next = [];

            foreach ((int r, int c) in frontier)
            {
                for (int i = 0; i < 4; i++)
                {
                    int nr = r + Dr[i];
                    int nc = c + Dc[i];

                    if (nr < 0 || nr >= this.Rows || nc < 0 || nc >= this.Columns)
                    {
                        continue;
                    }

                    if (this.Map[nr][nc] != '#')
                    {
                        next.Add((nr, nc));
                    }
                }
            }

            return next;
        }

        private HashSet<(int r, int c)> WalkInfinite(HashSet<(int r, int c)> frontier)
        {
            HashSet<(int r, int c)> next = [];

            foreach ((int r, int c) in frontier)
            {
                for (int i = 0; i < 4; i++)
                {
                    int nr = r + Dr[i];
                    int nc = c + Dc[i];

                    int rr = Mod(nr, this.Rows);
                    int cc = Mod(nc, this.Columns);

                    if (this.Map[rr][cc] != '#')
                    {
                        next.Add((nr, nc));
                    }
                }
            }

            return next;
        }

        private string[] BuildFixedFrame(
            HashSet<(int r, int c)> frontier,
            HashSet<(int r, int c)> everReached,
            int step,
            int totalSteps,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"STEP {step:000} OF {totalSteps:000} // REACHABLE NOW {frontier.Count:00000}");
            result.Add(string.Empty);

            for (int r = 0; r < this.Rows; r++)
            {
                StringBuilder sb = new();

                for (int c = 0; c < this.Columns; c++)
                {
                    char value = this.Map[r][c];

                    if ((r, c) == (this.Start.R, this.Start.C))
                    {
                        sb.Append(frontier.Contains((r, c)) ? '@' : 'S');
                    }
                    else if (frontier.Contains((r, c)))
                    {
                        sb.Append('O');
                    }
                    else if (everReached.Contains((r, c)))
                    {
                        sb.Append('o');
                    }
                    else
                    {
                        sb.Append(value == 'S' ? '.' : value);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildInfiniteWindowFrame(
            HashSet<(int r, int c)> frontier,
            HashSet<(int r, int c)> everReached,
            int step,
            int previewSteps,
            int radius,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"PREVIEW STEP {step:000} OF {previewSteps:000} // VISIBLE NOW {frontier.Count(p => Math.Abs(p.r - this.Start.R) <= radius && Math.Abs(p.c - this.Start.C) <= radius):00000}");
            result.Add("WINDOWED VIEW OF THE REPEATING FARM - NOT FULL 26501365 STEP FRAME");
            result.Add(string.Empty);

            int minR = this.Start.R - radius;
            int maxR = this.Start.R + radius;
            int minC = this.Start.C - radius;
            int maxC = this.Start.C + radius;

            for (int r = minR; r <= maxR; r++)
            {
                StringBuilder sb = new();

                for (int c = minC; c <= maxC; c++)
                {
                    int rr = Mod(r, this.Rows);
                    int cc = Mod(c, this.Columns);
                    char value = this.Map[rr][cc];

                    if ((r, c) == (this.Start.R, this.Start.C))
                    {
                        sb.Append(frontier.Contains((r, c)) ? '@' : 'S');
                    }
                    else if (frontier.Contains((r, c)))
                    {
                        sb.Append('O');
                    }
                    else if (everReached.Contains((r, c)))
                    {
                        sb.Append('o');
                    }
                    else if (r != this.Start.R && c != this.Start.C && rr == this.Start.R && cc == this.Start.C)
                    {
                        sb.Append('.');
                    }
                    else if (rr == 0 || cc == 0)
                    {
                        sb.Append(value == '#' ? '#' : ',');
                    }
                    else
                    {
                        sb.Append(value == 'S' ? '.' : value);
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

        private static int Mod(int value, int modulo)
            => ((value % modulo) + modulo) % modulo;

        private static (int row, int col) FindStart(char target, string[] grid)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                int idx = grid[r].IndexOf(target);
                if (idx >= 0)
                    return (r, idx);
            }

            throw new Exception($"Character '{target}' not found in grid.");
        }

        private static long CountReach(string[] grid, int H, int W, int sr, int sc, int steps)
        {
            HashSet<(int r, int c)> current = new() { (sr, sc) };

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int s = 0; s < steps; s++)
            {
                HashSet<(int r, int c)> next = new();

                foreach (var (r, c) in current)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int nr = r + dr[i];
                        int nc = c + dc[i];

                        int rr = ((nr % H) + H) % H;
                        int cc = ((nc % W) + W) % W;

                        if (grid[rr][cc] != '#')
                        {
                            next.Add((nr, nc));
                        }
                    }
                }

                current = next;
            }

            return current.Count;
        }
    }
}
