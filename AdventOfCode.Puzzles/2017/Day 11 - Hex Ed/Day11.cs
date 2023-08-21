namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_11___Hex_Ed;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11(string file)
        {
            this.DayTitle = "Hex Ed";
            this.GetPuzzleData(file);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new HexEd(this.Input[0]).Distance}";

        public string Gold() => $"{new HexEd(this.Input[0]).Furthest}";
    }
}
