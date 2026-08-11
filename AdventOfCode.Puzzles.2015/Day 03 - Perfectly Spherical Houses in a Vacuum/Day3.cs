namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_03___Perfectly_Spherical_Houses_in_a_Vacuum;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2015, 3, "Perfectly Spherical Houses in a Vacuum")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PerfectlySphericalHousesInAVacuum(this.Input).Deliver().Houses.Count}";

        public string Gold() => $"{new PerfectlySphericalHousesInAVacuum(this.Input, true).Deliver().Houses.Count}";
    }
}
