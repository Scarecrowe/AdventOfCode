namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_23___Experimental_Emergency_Teleportation;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
            : base(2018, 23, "Experimental Emergency Teleportation")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ExperimentalEmergencyTeleportation(this.Input).LargestRadius()}";

        public string Gold() => $"{new ExperimentalEmergencyTeleportation(this.Input).DistanceOfLargestPoint()}";
    }
}
