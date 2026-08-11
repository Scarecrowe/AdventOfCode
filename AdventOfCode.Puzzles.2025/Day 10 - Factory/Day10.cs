namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_10___Factory;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
            : base(2025, 10, "Factory")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Factory(this.Input).TotalMinimumPresses()}";

        public string Gold() => $"{new Factory(this.Input).TotalReducedPresses()}";
    }
}
