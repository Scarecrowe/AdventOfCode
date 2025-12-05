namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_05___Supply_Stacks;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
        {
            this.DayTitle = "Supply Stacks";
            this.GetPuzzleData(5, this.DayTitle, StringSplitOptions.None);
        }

        public Day5(string[] input) => this.Input = input;

        public string Silver() => $"{SupplyStacks.Single(this.Input)}";

        public string Gold() => $"{SupplyStacks.Multiple(this.Input)}";
    }
}
