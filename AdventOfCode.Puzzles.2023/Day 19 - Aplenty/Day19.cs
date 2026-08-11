namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_19___Aplenty;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
            : base(2023, 19, "Aplenty", StringSplitOptions.None)
        {
        }

        public Day19(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Aplenty(this.Input).TotalRatings()}";

        public string Gold() => $"{new Aplenty(this.Input).CombinationRatings()}";
    }
}
