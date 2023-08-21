namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_19___Beacon_Scanner;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19(string file)
        {
            this.DayTitle = "Beacon Scanner";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new BeaconScanner(this.Input).BeaconCount()}";

        public string Gold() => $"{new BeaconScanner(this.Input).LargestDistance()}";
    }
}
