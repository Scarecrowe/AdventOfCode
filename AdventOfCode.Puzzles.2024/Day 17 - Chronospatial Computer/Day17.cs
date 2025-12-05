namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_17___Chronospatial_Computer;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
        {
            this.DayTitle = "Chronospatial Computer";
            this.GetPuzzleData(17, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ChronospatialComputer(this.Input).FinalOutput()}";

        public string Gold() => $"{new ChronospatialComputer(this.Input).LowestPostive()}";
    }
}
