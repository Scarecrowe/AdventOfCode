namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_22___Sand_Slabs;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
            : base(2023, 22, "Sand Slabs", StringSplitOptions.None)
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SandSlabs(this.Input).Fall()}";

        public string Gold() => $"{new SandSlabs(this.Input).Disintergrate()}";
    }
}
