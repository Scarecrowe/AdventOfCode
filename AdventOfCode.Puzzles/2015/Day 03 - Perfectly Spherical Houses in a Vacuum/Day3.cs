namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_03___Perfectly_Spherical_Houses_in_a_Vacuum;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3(string file)
        {
            this.DayTitle = "Perfectly Spherical Houses in a Vacuum";
            this.GetPuzzleData(file);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{new PerfectlySphericalHousesInAVacuum(this.Input).Deliver().Houses.Count}";

        public string Gold() => $"{new PerfectlySphericalHousesInAVacuum(this.Input, true).Deliver().Houses.Count}";
    }
}
