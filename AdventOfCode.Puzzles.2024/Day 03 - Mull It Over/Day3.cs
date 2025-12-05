namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_03___Mull_It_Over;
    using System.Text.RegularExpressions;

    public class Day3 : Puzzle, IPuzzle
    {

        public Day3()
        {
            this.DayTitle = "Mull It Over";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public string Silver() => $"{new MullItOver(this.Input, false).Calculate()}";

        public string Gold() => $"{new MullItOver(this.Input, true).Calculate()}";
    }
}
