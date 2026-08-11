namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_03___Gear_Ratios;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2023, 3, "Gear Ratios")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new GearRatios(this.Input).Count()}";

        public string Gold() => $"{new GearRatios(this.Input).Ratio()}";
    }
}
