namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_19___Linen_Layout;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
        {
            this.DayTitle = "Linen Layout";
            this.GetPuzzleData(19, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new LinenLayout(this.Input).Possible()}";

        public string Gold() => $"{new LinenLayout(this.Input).Arrangements()}";
    }
}
