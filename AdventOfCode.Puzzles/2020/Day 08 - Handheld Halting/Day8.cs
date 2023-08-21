namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_08___Handheld_Halting;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Handheld Halting";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new HandheldHalting(this.Input).Execute().Accumulator}";

        public string Gold() => $"{new HandheldHalting(this.Input).Fixed()}";
    }
}
