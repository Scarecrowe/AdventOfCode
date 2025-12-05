namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_01___Red_Nosed_Reports;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
        {
            this.DayTitle = "Red-Nosed Reports";
            this.GetPuzzleData(2, this.DayTitle);
        }

        public string Silver() => $"{new RedNosedReports(this.Input).Safe()}";

        public string Gold() => $"{new RedNosedReports(this.Input).Safe(true)}";
    }
}
