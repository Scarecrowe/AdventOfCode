namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_08___Memory_Maneuver;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Memory Maneuver";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new MemoryManeuver(this.Input[0]).BuildTree().MetadataSum}";

        public string Gold() => $"{new MemoryManeuver(this.Input[0]).BuildTree().RootValue()}";
    }
}
