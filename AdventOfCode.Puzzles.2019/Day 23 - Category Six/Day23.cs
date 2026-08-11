namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_23___Category_Six;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
            : base(2019, 23, "Category Six")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CategorySix(this.Input[0]).Run()}";

        public string Gold() => $"{new CategorySix(this.Input[0]).Run(true)}";
    }
}
