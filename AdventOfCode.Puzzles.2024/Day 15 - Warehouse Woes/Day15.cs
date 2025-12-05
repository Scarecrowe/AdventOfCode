namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_15___Warehouse_Woes;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
        {
            this.DayTitle = "Warehouse Woes";
            this.GetPuzzleData(15, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new WarehouseWoes(this.Input).GpsCoordinate()}";

        public string Gold() => $"{new WarehouseWoes(this.Input, true).GpsCoordinate()}";
    }
}
