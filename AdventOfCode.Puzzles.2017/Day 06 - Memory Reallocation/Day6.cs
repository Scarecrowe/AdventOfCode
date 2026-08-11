namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_06___Memory_Reallocation;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
            : base(2017, 6, "Memory Reallocation")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new MemoryReallocation(this.Input).Redistribute()}";

        public string Gold() => $"{new MemoryReallocation(this.Input).InifititeCycles()}";
    }
}
