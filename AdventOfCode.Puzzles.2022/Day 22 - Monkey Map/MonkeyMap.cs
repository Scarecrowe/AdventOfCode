namespace AdventOfCode.Puzzles._2022.Day_22___Monkey_Map
{
    using System.Text;
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

        public VectorArray<int, MonkeyMapState> Map { get; private set; }

        public Human Human { get; private set; }

        public List<string> Moves { get; private set; }

        public long Password() => this.Human.Password();

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
    }
}
