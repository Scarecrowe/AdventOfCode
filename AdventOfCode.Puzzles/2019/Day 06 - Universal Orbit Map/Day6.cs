namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_06___Universal_Orbit_Map;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6(string file)
        {
            this.DayTitle = "Universal Orbit Map";
            this.GetPuzzleData(file);
        }

        public Day6(string[] input) => this.Input = input;

        public string Silver() => $"{new UniversalOrbitMap(this.Input).DirectInDirectCount()}";

        public string Gold() => $"{new UniversalOrbitMap(this.Input).MinimumOrbitTransfer()}";
    }
}
