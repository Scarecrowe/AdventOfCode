namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_19___Not_Enough_Minerals;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19(string file)
        {
            this.DayTitle = "Not Enough Minerals";
            this.GetPuzzleData(file);
        }

        public Day19(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new GeodeCracker(this.Input).Run24()}";

        [Slow]
        public string Gold() => $"{new GeodeCracker(this.Input).Run32()}";
    }
}
