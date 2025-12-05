namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_09___Smoke_Basin;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Smoke Basin";
            this.GetPuzzleData(9, this.DayTitle);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new SmokeBasin(this.Input).SumOfRiskLevels()}";

        public string Gold() => $"{new SmokeBasin(this.Input).SumOfBasin()}";
    }
}
