namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_19___Go_With_The_Flow;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
            : base(2018, 19, "Go With The Flow")
        {
        }

        public Day19(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new GoWithTheFlow(this.Input).Run()}";

        public string Gold() => $"{GoWithTheFlow.Calculate()}";
    }
}
