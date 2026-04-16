namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_11___Reactor;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Reactor";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public string Silver() => $"{new Reactor(this.Input).CountAllPaths()}";

        public string Gold() => $"{new Reactor(this.Input).CountServerPaths()}";

    }
}
