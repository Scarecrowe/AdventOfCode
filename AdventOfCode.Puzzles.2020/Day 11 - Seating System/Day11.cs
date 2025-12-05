namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_11___Seating_System;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11()
        {
            this.DayTitle = "Seating System";
            this.GetPuzzleData(11, this.DayTitle, StringSplitOptions.None);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new SeatingSystem(this.Input).SeatCount()}";

        public string Gold() => $"{new SeatingSystem(this.Input).VisibleSeatCount()}";
    }
}
