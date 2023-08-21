namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_18___Settlers_of_The_North_Pole;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Reservoir Research";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{new SettlersOfTheNorthPole(this.Input).Cycle(10)}";

        [Slow]
        public string Gold() => $"{new SettlersOfTheNorthPole(this.Input).Cycle(1000000000)}";
    }
}
