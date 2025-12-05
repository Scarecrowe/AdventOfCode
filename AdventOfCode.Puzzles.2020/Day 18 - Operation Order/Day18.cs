namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_18___Operation_Order;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
        {
            this.DayTitle = "Operation Order";
            this.GetPuzzleData(18, this.DayTitle);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{OperationOrder.Simple(this.Input)}";

        public string Gold() => $"{OperationOrder.Advanced(this.Input)}";
    }
}
