namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_03___Gear_Ratios;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
        {
            this.DayTitle = "Gear Ratios";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public string Silver() => $"{new GearRatios(this.Input).Count()}";

        public string Gold() => $"{new GearRatios(this.Input).Ratio()}";
    }
}
