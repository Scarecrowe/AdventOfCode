namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_25___The_Halting_Problem;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
            : base(2017, 25, "The Halting Problem", StringSplitOptions.None)
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TuringMachine(this.Input).Run().CountOnes()}";

        public string Gold() => $"You have enough stars to [Reboot the Printer]";
    }
}
