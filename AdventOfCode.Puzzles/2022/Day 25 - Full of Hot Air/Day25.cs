namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_25___Full_of_Hot_Air;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25(string file)
        {
            this.DayTitle = "Full of Hot Air";
            this.GetPuzzleData(file);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{FullOfHotAir.Snafu(this.Input)}";

        public string Gold() => $"You have enough stars to [Start The Blender]";
    }
}
