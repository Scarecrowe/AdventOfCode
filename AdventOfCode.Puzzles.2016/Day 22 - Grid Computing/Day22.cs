namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_22___Grid_Computing;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
            : base(2016, 22, "Grid Computing")
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new GridComputing(this.Input).FindAvailablePairs()}";

        public string Gold() => $"{new GridComputing(this.Input).MoveData()}";
    }
}
