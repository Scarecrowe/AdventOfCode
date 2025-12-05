namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_11___Cosmic_Expansion;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Cosmic Expansion";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public string Silver() => $"{new CosmicExpansion(this.Input).SumOfShortestPath(2)}";

        public string Gold() => $"{new CosmicExpansion(this.Input).SumOfShortestPath(1000000)}";
    }
}
