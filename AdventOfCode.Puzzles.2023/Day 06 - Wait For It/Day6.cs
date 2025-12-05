namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_06___Wait_For_It;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
        {
            this.DayTitle = "Wait For It";
            this.GetPuzzleData(6, this.DayTitle);
        }

        public string Silver() => $"{new WaitForIt(this.Input).Race()}";

        public string Gold() => $"{new WaitForIt(this.Input, true).Race()}";
    }
}
