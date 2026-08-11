namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_25___Snowverload;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
            : base(2023, 25, "Snowverload", StringSplitOptions.None)
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Snowverload(this.Input).BigRedReset()}";

        public string Gold() => $"Push The Big Red Button Again";
    }
}
