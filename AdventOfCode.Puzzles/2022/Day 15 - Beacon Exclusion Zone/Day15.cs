namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_15___Beacon_Exclusion_Zone;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Beacon Exclusion Zone";
            this.GetPuzzleData(file);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new BeaconExclusionZone(this.Input).NonBeacon()}";

        [Slow]
        public string Gold() => $"{new BeaconExclusionZone(this.Input).TuningFrequency()}";
    }
}
