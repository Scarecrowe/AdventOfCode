namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_14___Extended_Polymerization;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14(string file)
        {
            this.DayTitle = "Extended Polymerization";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day14(string[] input) => this.Input = input;

        public string Silver() => $"{new ExtendedPolymerization(this.Input).Process(10)}";

        public string Gold() => $"{new ExtendedPolymerization(this.Input).Process(40)}";
    }
}
