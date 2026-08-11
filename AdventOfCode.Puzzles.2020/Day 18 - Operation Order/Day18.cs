namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_18___Operation_Order;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
            : base(2020, 18, "Operation Order")
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{OperationOrder.Simple(this.Input)}";

        public string Gold() => $"{OperationOrder.Advanced(this.Input)}";
    }
}
