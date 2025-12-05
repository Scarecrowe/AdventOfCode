namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_19___An_Elephant_Named_Joseph;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
        {
            this.DayTitle = "An Elephant Named Joseph";
            this.GetPuzzleData(19, this.DayTitle);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new AnElephantNamedJoseph(this.Input).Clockwise()}";

        public string Gold() => $"{new AnElephantNamedJoseph(this.Input).Opposite()}";
    }
}
