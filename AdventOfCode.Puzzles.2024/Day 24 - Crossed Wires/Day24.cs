namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_24___Crossed_Wires;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
            : base(2024, 24, "Crossed Wires", StringSplitOptions.None)
        {
        }

        public string Silver() => $"{new CrossedWires(this.FilePath).ZOutput()}";

        public string Gold() => $"{new CrossedWires(this.FilePath).WireNames()}";
    }
}
