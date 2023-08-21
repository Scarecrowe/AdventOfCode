namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_17___Trick_Shot;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Trick Shot";
            this.GetPuzzleData(file);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new TrickShot(this.Input).Simulate(true)}";

        public string Gold() => $"{new TrickShot(this.Input).Simulate(false)}";
    }
}
