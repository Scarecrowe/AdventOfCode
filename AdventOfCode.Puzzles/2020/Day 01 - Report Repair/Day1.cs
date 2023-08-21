namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_01___Report_Repair;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1(string file)
        {
            this.DayTitle = "Report Repair";
            this.GetPuzzleData(file);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new ReportRepair(this.Input).Pair()}";

        public string Gold() => $"{new ReportRepair(this.Input).Tripple()}";
    }
}
