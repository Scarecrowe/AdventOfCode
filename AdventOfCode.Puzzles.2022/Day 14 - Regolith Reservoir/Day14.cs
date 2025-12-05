namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_14___Regolith_Reservoir;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
        {
            this.DayTitle = "Regolith Reservoir";
            this.GetPuzzleData(14, this.DayTitle);
        }

        public Day14(string[] input) => this.Input = input;

        public string Silver() => $"{new RegolithReservoir(this.Input, false).Run().SumOfSand()}";

        public string Gold() => $"{new RegolithReservoir(this.Input, true).Run().SumOfSand()}";
    }
}
