namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_21___Chronal_Conversion;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21()
            : base(2018, 21, "Chronal Conversion")
        {

        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ChronalConversion(this.Input).LowestNonNegative()}";

        public string Gold() => $"{new ChronalConversion(this.Input).MostNonNegative()}";
    }
}
