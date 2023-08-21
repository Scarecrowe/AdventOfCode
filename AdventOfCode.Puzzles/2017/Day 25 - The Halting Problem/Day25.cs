namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_25___The_Halting_Problem;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25(string file)
        {
            this.DayTitle = "The Halting Problem";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day25(string[] input) => this.Input = input;

        public string Silver() => $"{new TuringMachine(this.Input).Run().CountOnes()}";

        public string Gold() => $"You have enough stars to [Reboot the Printer]";
    }
}
