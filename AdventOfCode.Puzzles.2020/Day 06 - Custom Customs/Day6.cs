namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_06___Custom_Customs;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
            : base(2020, 6, "Custom Customs", StringSplitOptions.None)
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{CustomCustoms.Anyone(this.Input)}";

        public string Gold() => $"{CustomCustoms.Everyone(this.Input)}";
    }
}
