namespace AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin
{
    using AdventOfCode.Core;

    public class Blizzard
    {
        public Blizzard(Vector<int> point, Vector<int> direction, int row, int column)
        {
            this.Point = point;
            this.Direction = direction;
            this.Row = row;
            this.Column = column;
        }

        public static Dictionary<char, Vector<int>> Directions { get; } = new()
        {
            { '>', new(1, 0) },
            { '<', new(-1, 0) },
            { '^', new(0, -1) },
            { 'v', new(0, 1) },
            { '#', new(0, 0) },
            { '.', new(0, 0) }
        };

        public Vector<int> Point { get; private set; }

        public Vector<int> Direction { get; }

        public int Row { get; }

        public int Column { get; }

        public static List<Blizzard> Parse(string[] input)
        {
            int y = input.Length;
            int x = input[0].Length;

            List<Blizzard> result = new();

            for (var i = 0; i < y; i++)
            {
                for (var j = 0; j < x; j++)
                {
                    if (Directions[input[i][j]] != new Vector<int>(0, 0))
                    {
                        result.Add(new Blizzard(new(j, i), Directions[input[i][j]], y, x));
                    }
                }
            }

            return result;
        }

        public Blizzard Move()
        {
            int y = this.Point.Y + this.Direction.Y;

            if (y == 0)
            {
                y = this.Row - 2;
            }
            else if (y == this.Row - 1)
            {
                y = 1;
            }

            int x = this.Point.X + this.Direction.X;

            if (x == 0)
            {
                x = this.Column - 2;
            }
            else if (x == this.Column - 1)
            {
                x = 1;
            }

            this.Point = new Vector<int>(x, y);

            return this;
        }
    }
}
