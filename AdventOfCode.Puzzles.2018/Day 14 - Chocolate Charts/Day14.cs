namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_14___Chocolate_Charts;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
            : base(2018, 14, "Chocolate Charts")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ChocolateCharts(this.Input[0]).NextTenRecipes()}";

        public string Gold() => $"{new ChocolateCharts(this.Input[0]).IndexOfRecipe()}";
    }
}
