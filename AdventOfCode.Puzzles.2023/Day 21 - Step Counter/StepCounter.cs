namespace AdventOfCode.Puzzles._2023.Day_21___Step_Counter
{
    public class StepCounter
    {
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
