namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_18___RAM_Run;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18()
            : base(2024, 18, "RAM Run", StringSplitOptions.None)
        {
        }

        public Day18(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RamRun(this.Input).ShortestPath()}";

        public string Gold() => $"{new RamRun(this.Input).NonReachable()}";
    }
}
