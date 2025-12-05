namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_23___Category_Six;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
        {
            this.DayTitle = "Category Six";
            this.GetPuzzleData(23, this.DayTitle);
        }

        public Day23(string[] input) => this.Input = input;

        public string Silver() => $"{new CategorySix(this.Input[0]).Run()}";

        public string Gold() => $"{new CategorySix(this.Input[0]).Run(true)}";
    }
}
