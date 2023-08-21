namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_18___Snailfish;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Snailfish";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{new SnailFishMaths(this.Input).FinalMagnitude()}";

        public string Gold() => $"{new SnailFishMaths(this.Input).LargestMagnitude()}";
    }
}
