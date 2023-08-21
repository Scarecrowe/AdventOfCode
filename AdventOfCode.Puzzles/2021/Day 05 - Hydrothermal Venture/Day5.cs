namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_05___Hydrothermal_Venture;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5(string file)
        {
            this.DayTitle = "Hydrothermal Venture";
            this.GetPuzzleData(file);
        }

        public Day5(string[] input) => this.Input = input;

        public string Silver() => $"{new HydrothermalVenture(this.Input, false).TotalCrossOverVents()}";

        public string Gold() => $"{new HydrothermalVenture(this.Input, true).TotalCrossOverVents()}";
    }
}
