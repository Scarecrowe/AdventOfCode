namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_15___Dueling_Generators;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
            : base(2017, 15, "Dueling Generators")
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new DuelingGenerators(this.Input).Simple()}";

        public string Gold() => $"{new DuelingGenerators(this.Input).Advanced()}";
    }
}
