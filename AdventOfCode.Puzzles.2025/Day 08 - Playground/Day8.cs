namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_08___Playground;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
            : base(2025, 8, "Playground")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Playground(this.Input).MultiplyTopThreeCircuit()}";

        public string Gold() => $"{new Playground(this.Input).MultiplyXCoordinates()}";
    }
}
