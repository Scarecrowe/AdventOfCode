namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_11___Corporate_Policy;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Corporate Policy";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{CooperatePolicy.Generate(this.Input[0])}";

        public string Gold() => $"{CooperatePolicy.Generate(CooperatePolicy.Generate(this.Input[0]))}";
    }
}
