namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_18___Lavaduct_Lagoon;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
        {
            this.DayTitle = "Lavaduct Lagoon";
            this.GetPuzzleData(18, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new LavaductLagoon(this.Input).Small()}";

        public string Gold() => $"{new LavaductLagoon(this.Input).Large()}";
    }
}
