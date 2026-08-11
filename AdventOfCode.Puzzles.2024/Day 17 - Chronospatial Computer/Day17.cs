namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_17___Chronospatial_Computer;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
            : base(2024, 17, "Chronospatial Computer", StringSplitOptions.None)
        {
        }

        public Day17(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ChronospatialComputer(this.Input).FinalOutput()}";

        public string Gold() => $"{new ChronospatialComputer(this.Input).LowestPostive()}";
    }
}
