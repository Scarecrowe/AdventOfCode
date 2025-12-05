namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_01___Historian_Hysteria;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Historian Hysteria";
            this.GetPuzzleData(1, this.DayTitle);
        }

        public string Silver() => $"{new HistorianHysteria(this.Input).Distance()}";

        public string Gold() => $"{new HistorianHysteria(this.Input).Similarity()}";
    }
}
