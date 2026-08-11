namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_02___Red_Nosed_Reports;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2024, 2, "Red-Nosed Reports")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RedNosedReports(this.Input).Safe()}";

        public string Gold() => $"{new RedNosedReports(this.Input).Safe(true)}";
    }
}
