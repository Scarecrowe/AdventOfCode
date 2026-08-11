namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_22___Monkey_Market;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
            : base(2024, 22, "Monkey Market", StringSplitOptions.None)
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new MonkeyMarket(this.Input).Sum()}";

        public string Gold() => $"{new MonkeyMarket(this.Input).Best()}";
    }
}
