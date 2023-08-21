namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_02___Inventory_Management_System;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Inventory Management System";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{new InventoryManagementSystem(this.Input).Checksum()}";

        public string Gold() => $"{new InventoryManagementSystem(this.Input).PrototypeFabric()}";
    }
}
