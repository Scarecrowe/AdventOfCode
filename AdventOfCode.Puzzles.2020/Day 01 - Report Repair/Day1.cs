namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_01___Report_Repair;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
            : base(2020, 1, "Report Repair")
        {
        }

        public Day1(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ReportRepair(this.Input).Pair()}";

        public string Gold() => $"{new ReportRepair(this.Input).Tripple()}";
    }
}
