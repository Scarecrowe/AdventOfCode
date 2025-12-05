namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_06___Guard_Gallivant;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
        {
            this.DayTitle = "Guard Gallivant";
            this.GetPuzzleData(6, this.DayTitle);
        }

        public string Silver() => $"{new GuardGallivant(this.Input).WithoutCollisions()}";

        public string Gold() => $"{new GuardGallivant(this.Input).WithCollisions()}";
    }
}
