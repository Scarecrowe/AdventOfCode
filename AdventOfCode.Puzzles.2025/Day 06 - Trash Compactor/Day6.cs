namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_06___Trash_Compactor;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6()
            : base(2025, 5, "Trash Compactor")
        {
        }

        public Day6(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TrashCompactor(this.Input).Sum()}";

        public string Gold() => $"{new TrashCompactor(this.Input).RightToLeftSum()}";
    }
}
