namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_24___Electromagnetic_Moat;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "Electromagnetic Moat";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new ElectromagneticMoat(this.Input).BuildBridges().Strongest}";

        public string Gold() => $"{new ElectromagneticMoat(this.Input).BuildBridges().Longest}";
    }
}
