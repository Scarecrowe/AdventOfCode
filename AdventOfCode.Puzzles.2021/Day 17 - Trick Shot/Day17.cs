namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_17___Trick_Shot;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
        {
            this.DayTitle = "Trick Shot";
            this.GetPuzzleData(17, this.DayTitle);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new TrickShot(this.Input).Simulate(true)}";

        public string Gold() => $"{new TrickShot(this.Input).Simulate(false)}";
    }
}
