namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_02___Inventory_Management_System;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2018, 2, "Inventory Management System")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new InventoryManagementSystem(this.Input).Checksum()}";

        public string Gold() => $"{new InventoryManagementSystem(this.Input).PrototypeFabric()}";
    }
}
