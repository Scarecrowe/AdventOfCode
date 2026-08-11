namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_24___Never_Tell_Me_The_Odds;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
            : base(2023, 24, "Never Tell Me The Odds", StringSplitOptions.None)
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new NeverTellMeTheOdds(this.Input).TestAreaIntersections()}";

        public string Gold() => $"{new NeverTellMeTheOdds(this.Input, false).SingleThrow()}";
    }
}
