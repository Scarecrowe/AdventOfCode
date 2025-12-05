namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_22___Sand_Slabs;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Sand Slabs";
            this.GetPuzzleData(22, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new SandSlabs(this.Input).Fall()}";

        public string Gold() => $"{new SandSlabs(this.Input).Disintergrate()}";
    }
}
