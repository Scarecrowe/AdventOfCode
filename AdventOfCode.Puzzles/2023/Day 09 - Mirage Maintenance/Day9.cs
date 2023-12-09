namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_09___Mirage_Maintenance;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Mirage Maintenance";
            this.GetPuzzleData(file);
        }

        public string Silver() => $"{new MirageMaintenance(this.Input).End()}";

        public string Gold() => $"{new MirageMaintenance(this.Input).Beginning()}";
    }
}
