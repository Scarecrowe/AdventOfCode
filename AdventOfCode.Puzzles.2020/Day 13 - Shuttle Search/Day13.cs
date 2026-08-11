namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_13___Shuttle_Search;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
            : base(2020, 13, "Shuttle Search")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{ShuttleSearch.EarliestBusId(this.Input)}";

        public string Gold() => $"{ShuttleSearch.EarliestTimestamp(this.Input)}";
    }
}
