namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_18___Boiling_Boulders;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Boiling Boulders";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new BoilingBoulders(this.Input).SurfaceArea()}";

        [Slow]
        public string Gold() => $"{new BoilingBoulders(this.Input).ExteriorSurfaceArea()}";
    }
}
