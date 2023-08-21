namespace AdventOfCode.Puzzles._2015.Day_25___Let_It_Snow
{
    using AdventOfCode.Core.Extensions;

    public class LetItSnow
    {
        private const long Mulitplier = 252533;

        private const long Divisor = 33554393;

        public LetItSnow(string[] input)
        {
            string[] tokens = input[0].Split("  ");
            tokens = tokens[1].SplitSpace();

            this.Row = tokens[5][..^1].ToInt();
            this.Column = tokens[7][..^1].ToInt();
        }

        private int Row { get; }

        private int Column { get; }

        public long Generate()
        {
            long value = 20151125;
            int y = 1;
            int x = 1;

            while (true)
            {
                if (this.Row == y && this.Column == x)
                {
                    break;
                }

                y--;
                x++;

                if (y == 0)
                {
                    y = x;
                    x = 1;
                }

                value = (value * Mulitplier) % Divisor;
            }

            return value;
        }
    }
}
