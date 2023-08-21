namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_11___Seating_System;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11(string file)
        {
            this.DayTitle = "Seating System";
            this.GetPuzzleData(file, StringSplitOptions.None);

            // this.Input = new string[10];
            // this.Input[0] = "L.LL.LL.LL";
            // this.Input[1] = "LLLLLLL.LL";
            // this.Input[2] = "L.L.L..L..";
            // this.Input[3] = "LLLL.LL.LL";
            // this.Input[4] = "L.LL.LL.LL";
            // this.Input[5] = "L.LLLLL.LL";
            // this.Input[6] = "..L.L.....";
            // this.Input[7] = "LLLLLLLLLL";
            // this.Input[8] = "L.LLLLLL.L";
            // this.Input[9] = "L.LLLLL.LL";
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new SeatingSystem(this.Input).SeatCount()}";

        public string Gold() => $"{new SeatingSystem(this.Input).VisibleSeatCount()}";
    }
}
