namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Reservoir Research";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{new ReservoirResearch(this.Input).Settle(true)}";

        public string Gold() => $"{new ReservoirResearch(this.Input).Settle(false)}";
    }
}
