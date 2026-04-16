namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_12___Christmas_Tree_Farm;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12()
        {
            this.DayTitle = "Christmas Tree Farm";
            this.GetPuzzleData(12, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ChristmasTreeFarm(this.Input).RegionCount()}";

        public string Gold() => $"Finish Decorating the North Pole";
    }
}
