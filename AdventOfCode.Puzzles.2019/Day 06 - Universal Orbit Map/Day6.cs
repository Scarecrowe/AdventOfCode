namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_06___Universal_Orbit_Map;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
            : base(2019, 6, "Universal Orbit Map")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new UniversalOrbitMap(this.Input).DirectInDirectCount()}";

        public string Gold() => $"{new UniversalOrbitMap(this.Input).MinimumOrbitTransfer()}";
    }
}
