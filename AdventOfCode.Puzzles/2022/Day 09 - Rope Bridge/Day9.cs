namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_09___Rope_Bridge;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Rope Bridge";
            this.GetPuzzleData(file);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new RopeBridge(this.Input, 2).Visited()}";

        public string Gold() => $"{new RopeBridge(this.Input, 10).Visited()}";
    }
}
