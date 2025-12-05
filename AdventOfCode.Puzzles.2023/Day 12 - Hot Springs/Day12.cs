namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_12___Hot_Springs;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12()
        {
            this.DayTitle = "Hot Springs";
            this.GetPuzzleData(12, this.DayTitle);
        }

        public string Silver() => $"{new HotSprings(this.Input)}";

        public string Gold() => $"{new HotSprings(this.Input)}";
    }
}
