namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_06___Chronal_Coordinates;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6(string file)
        {
            this.DayTitle = "Chronal Coordinates";
            this.GetPuzzleData(file);
        }

        public Day6(string[] input) => this.Input = input;

        [Slow]
        public string Silver() => $"{new ChronalCoordinates(this.Input).Dangerous()}";

        [Slow]
        public string Gold() => $"{new ChronalCoordinates(this.Input).Safe(10000)}";
    }
}
