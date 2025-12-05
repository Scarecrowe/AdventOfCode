namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_25___Code_Chronicle;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
        {
            this.DayTitle = "Code Chronicle";
            this.GetPuzzleData(25, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new CodeChronicle(this.Input).Unique()}";

        public string Gold() => $"Deliver The Chronicle";
    }
}
