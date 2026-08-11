namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_19___Linen_Layout;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
            : base(2024, 19, "Linen Layout", StringSplitOptions.None)
        {
        }

        public Day19(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new LinenLayout(this.Input).Possible()}";

        public string Gold() => $"{new LinenLayout(this.Input).Arrangements()}";
    }
}
