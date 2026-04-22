namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_04___Ceres_Search;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
        {
            this.DayTitle = "Ceres Search";
            this.GetPuzzleData(4, this.DayTitle);
        }

        public string Silver() => $"{new CeresSearch(this.Input).XmasCount()}";

        public string Gold() => $"{new CeresSearch(this.Input).XmasHyphenCount()}";
    }
}
