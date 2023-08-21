namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_18___Operation_Order;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Operation Order";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{OperationOrder.Simple(this.Input)}";

        public string Gold() => $"{OperationOrder.Advanced(this.Input)}";
    }
}
