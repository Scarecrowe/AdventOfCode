namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_23___Coprocessor_Conflagration;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
            : base(2017, 23, "Coprocessor Conflagration")
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new CoprocessorConflagration(this.Input).Simple()}";

        public string Gold() => $"{new CoprocessorConflagration(this.Input).Advanced()}";
    }
}
