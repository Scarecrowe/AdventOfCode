namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "Blizzard Basin";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new BlizzardBasin(this.Input).FewestMinutes()}";

        public string Gold() => $"{new BlizzardBasin(this.Input).FewestMinutesWithRoundTrip()}";
    }
}
