namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_11___Hex_Ed;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
            : base(2017, 11, "Hex Ed")
        {
        }

        public Day11(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new HexEd(this.Input[0]).Distance}";

        public string Gold() => $"{new HexEd(this.Input[0]).Furthest}";
    }
}
