namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_14___Restroom_Redoubt;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
            : base(2024, 14, "Restroom Redoubt")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RestroomRedoubt(this.Input).SafetyFactor(100)}";

        public string Gold() => $"{new RestroomRedoubt(this.Input).XmasTree()}";
    }
}
