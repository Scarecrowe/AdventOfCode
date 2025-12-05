namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_18___RAM_Run;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
        {
            this.DayTitle = "RAM Run";
            this.GetPuzzleData(18, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new RamRun(this.Input).ShortestPath()}";

        public string Gold() => $"{new RamRun(this.Input).NonReachable()}";
    }
}
