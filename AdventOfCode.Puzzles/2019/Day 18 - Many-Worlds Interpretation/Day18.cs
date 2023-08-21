namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_18___Many_Worlds_Interpretation;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Many-Worlds Interpretation";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new ManyWorldsInterpretation(this.Input).CollectKeys()}";

        public string Gold() => $"{new ManyWorldsInterpretation(this.Input).SplitMap().CollectVaultKeys()}";
    }
}
