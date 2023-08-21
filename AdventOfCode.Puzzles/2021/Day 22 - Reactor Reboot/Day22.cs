namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_22___Reactor_Reboot;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22(string file)
        {
            this.DayTitle = "Reactor Reboot";
            this.GetPuzzleData(file);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new ReactorReboot(this.Input).Reboot()}";

        public string Gold() => $"{new ReactorReboot(this.Input).Reboot(false)}";
    }
}
