namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_19___Aplenty;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
        {
            this.DayTitle = "Aplenty";
            this.GetPuzzleData(19, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new Aplenty(this.Input).TotalRatings()}";

        public string Gold() => $"{new Aplenty(this.Input).CombinationRatings()}";
    }
}
