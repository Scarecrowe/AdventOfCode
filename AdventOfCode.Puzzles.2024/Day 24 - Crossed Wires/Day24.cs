namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_24___Crossed_Wires;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
        {
            this.DayTitle = "Crossed Wires";
            this.GetPuzzleData(24, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new CrossedWires(this.FilePath).ZOutput()}";

        public string Gold() => $"{new CrossedWires(this.FilePath).WireNames()}";
    }
}
