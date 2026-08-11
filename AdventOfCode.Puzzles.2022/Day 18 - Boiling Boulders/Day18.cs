namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_18___Boiling_Boulders;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
            : base(2022, 18, "Boiling Boulders")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        [Slow]
        public string Silver() => $"{new BoilingBoulders(this.Input).SurfaceArea()}";

        [Slow]
        public string Gold() => $"{new BoilingBoulders(this.Input).ExteriorSurfaceArea()}";
    }
}
