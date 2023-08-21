namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_12___Passage_Pathing;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Passage Pathing";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new PassagePathing(this.Input).UniquePathCount()}";

        [Slow]
        public string Gold() => $"{new PassagePathing(this.Input).UniquePathCount(true)}";
    }
}
