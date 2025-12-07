namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_07___Laboratories;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "Laboratories";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public string Silver() => $"{new Laboratories(this.Input).SplitBeams()}";

        public string Gold() => $"{new Laboratories(this.Input).Timelines()}";
    }
}
