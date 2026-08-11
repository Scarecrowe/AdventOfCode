namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_08___Handheld_Halting;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
            : base(2020, 8, "Handheld Halting")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HandheldHalting(this.Input).Execute().Accumulator}";

        public string Gold() => $"{new HandheldHalting(this.Input).Fixed()}";
    }
}
