namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_10___Knot_Hash;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
        {
            this.DayTitle = "Knot Hash";
            this.GetPuzzleData(10, this.DayTitle);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{KnotHash.Product(this.Input[0])}";

        public string Gold() => $"{KnotHash.Hash(this.Input[0])}";
    }
}
