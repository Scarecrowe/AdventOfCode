namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_11___Radioisotope_Thermoelectric_Generators;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Radioisotope Thermoelectric Generators";
            this.GetPuzzleData(11, this.DayTitle);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new RadioisotopeThermoelectricGenerators(this.Input).MinimumSteps()}";

        public string Gold() => $"{new RadioisotopeThermoelectricGenerators(this.Input, true).MinimumSteps()}";
    }
}
