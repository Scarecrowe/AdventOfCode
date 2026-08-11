namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_05___Binary_Boarding;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2020, 5, "Binary Boarding", StringSplitOptions.None)
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new BinaryBoarding(this.Input).Process().MaxSeatId()}";

        public string Gold() => $"{new BinaryBoarding(this.Input).Process().MissingSeatId()}";
    }
}
