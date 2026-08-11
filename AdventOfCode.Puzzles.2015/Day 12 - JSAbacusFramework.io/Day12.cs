namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_12___JSAbacusFramework.io;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12()
            : base(2015, 12, "JSAbacusFramework.io")
        {
        }

        public Day12(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{JSAbacusFramework.SumAll(this.Input[0])}";

        public string Gold() => $"{JSAbacusFramework.SumNonRed(this.Input[0])}";
    }
}
