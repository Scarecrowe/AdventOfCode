namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_05___Cafeteria;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
        {
            this.DayTitle = "Cafeteria";
            this.GetPuzzleData(5, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new Cafeteria(this.Input).AvailableCount()}";

        public string Gold() => $"{new Cafeteria(this.Input).FreshCount()}";
    }
}
