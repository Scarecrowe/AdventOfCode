namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_03___Binary_Diagnostic;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
        {
            this.DayTitle = "Binary Diagnostic";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{BinaryDiagnostic.PowerConsumption(this.Input)}";

        public string Gold() => $"{BinaryDiagnostic.LifeSupport(this.Input)}";
    }
}
