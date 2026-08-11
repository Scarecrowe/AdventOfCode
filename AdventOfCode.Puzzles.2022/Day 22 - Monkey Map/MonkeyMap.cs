namespace AdventOfCode.Puzzles._2022.Day_22___Monkey_Map
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class MonkeyMap
    {
        public MonkeyMap(string[] input)
        {
            int splitIndex = 0;
            int width = 0;

            for (int y = 0; y < input.Length; y++)
            {
                if (input[y].Length > width)
                {
                    width = input[y].Length;
                }

                if (string.IsNullOrEmpty(input[y]))
                {
                    splitIndex = y;
                    break;
                }
            }

            this.Map = new(width, splitIndex);
            this.Moves = new();

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case '.':
                            this.Map[y, x] = MonkeyMapState.Open;
                            break;
                        case '#':
                            this.Map[y, x] = MonkeyMapState.Closed;
                            break;
                    }
                }
            }

            StringBuilder move = new();

            for (long y = this.Map.Height + 1; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    switch (input[y][x])
                    {
                        case 'L':
                        case 'R':
                            this.Moves.Add(move.ToString());
                            this.Moves.Add(input[y][x].ToString());
                            move.Clear();
                            break;
                        default:
                            move.Append(input[y][x]);
                            break;
                    }
                }
            }

            this.Moves.Add(move.ToString());
            this.Human = new(this.Map);
        }

        public MonkeyMap(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public VectorArray<int, MonkeyMapState> Map { get; private set; }

        public Human Human { get; private set; }

        public List<string> Moves { get; private set; }

        public long Password() => this.Human.Password();

        public MonkeyMap Navigate(MonkeyMapType mapType)
        {
            foreach (string move in this.Moves)
            {
                if (move == "R" || move == "L")
                {
                    this.Human.Turn(move).AddVisit();
                    continue;
                }

                if (mapType == MonkeyMapType.TwoDimensional)
                {
                    this.Human.MoveTwoDimensional(move.ToInt(), this.Map);
                }
                else
                {
                    this.Human.MoveThreeDimensional(move.ToInt(), this.Map);
                }
            }

            return this;
        }

        public MonkeyMap RenderSilver(int renderEvery = 4)
            => this.Render(MonkeyMapType.TwoDimensional, renderEvery);

        public MonkeyMap RenderGold(int renderEvery = 4)
            => this.Render(MonkeyMapType.ThreeDimensional, renderEvery);

        private MonkeyMap Render(MonkeyMapType mapType, int renderEvery)
        {
            int step = 0;

            this.RenderFrame(mapType, step);

            foreach (string move in this.Moves)
            {
                if (move == "R" || move == "L")
                {
                    this.Human.Turn(move).AddVisit();

                    step++;

                    if (step % renderEvery == 0)
                    {
                        this.RenderFrame(mapType, step);
                    }

                    continue;
                }

                int spaces = move.ToInt();

                for (int i = 0; i < spaces; i++)
                {
                    Vector<int> before = new(this.Human.Point);

                    if (mapType == MonkeyMapType.TwoDimensional)
                    {
                        this.Human.MoveTwoDimensional(1, this.Map);
                    }
                    else
                    {
                        this.Human.MoveThreeDimensional(1, this.Map);
                    }

                    step++;

                    if (step % renderEvery == 0 || before == this.Human.Point)
                    {
                        this.RenderFrame(mapType, step);
                    }
                }
            }

            this.RenderFrame(mapType, step, true);

            return this;
        }

        private void RenderFrame(MonkeyMapType mapType, int step, bool complete = false)
        {
            this.Renderer?.RenderFrame(new Frame(this.BuildFrame(mapType, step, complete)));
        }

        private string[] BuildFrame(MonkeyMapType mapType, int step, bool complete)
        {
            List<string> result = [];

            result.Add(
                complete
                    ? $"MONKEY MAP // {(mapType == MonkeyMapType.TwoDimensional ? "FLAT" : "CUBE")} // PASSWORD {this.Password()}"
                    : $"MONKEY MAP // {(mapType == MonkeyMapType.TwoDimensional ? "FLAT" : "CUBE")} // STEP {step:00000}");

            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == this.Human.Point)
                    {
                        sb.Append('@');
                        continue;
                    }

                    if (this.Human.Visited.TryGetValue(point, out Cardinal facing))
                    {
                        sb.Append(CardinalHelper.CardinalToSymbolMap[facing]);
                        continue;
                    }

                    sb.Append(this.Map[y, x] switch
                    {
                        MonkeyMapState.Void => ' ',
                        MonkeyMapState.Open => '.',
                        MonkeyMapState.Closed => '#',
                        _ => ' '
                    });
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        public MonkeyMap PrintMap()
        {
            for (int x = 0; x < this.Map.Width; x++)
            {
                if (x == 0 || x % 25 == 0)
                {
                    PuzzleConsole.Write($"{x}");
                    x += x.ToString().Length - 1;
                    continue;
                }

                PuzzleConsole.Write(" ");
            }

            PuzzleConsole.WriteLine();

            for (int y = 0; y < this.Map.Height; y++)
            {
                if (y == 0 || y % 25 == 0)
                {
                    PuzzleConsole.Write($"{y,3}");
                }
                else
                {
                    PuzzleConsole.Write("   ");
                }

                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Human.Visited.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(CardinalHelper.CardinalToSymbolMap[this.Human.Visited[new(x, y)]]);
                        continue;
                    }

                    switch (this.Map[y, x])
                    {
                        case MonkeyMapState.Void:
                            PuzzleConsole.Write(" ");
                            break;

                        case MonkeyMapState.Open:
                            PuzzleConsole.Write(".");
                            break;

                        case MonkeyMapState.Closed:
                            PuzzleConsole.Write("#");
                            break;
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();

            return this;
        }
    }
}