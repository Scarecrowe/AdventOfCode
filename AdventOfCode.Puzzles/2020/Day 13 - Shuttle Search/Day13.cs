namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_13___Shuttle_Search;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13(string file)
        {
            this.DayTitle = "Shuttle Search";
            this.GetPuzzleData(file);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{ShuttleSearch.EarliestBusId(this.Input)}";

        public string Gold() => $"{ShuttleSearch.EarliestTimestamp(this.Input)}";
    }
}
