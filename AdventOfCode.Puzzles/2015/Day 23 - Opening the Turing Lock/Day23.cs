namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_23___Opening_the_Turing_Lock;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23(string file)
        {
            this.DayTitle = "Opening the Turing Lock";
            this.GetPuzzleData(file);
        }

        public Day23(string[] input) => this.Input = input;

        public string Silver() => $"{new OpeningTheTuringLock(this.Input).Execute().RegisterB()}";

        public string Gold() => $"{new OpeningTheTuringLock(this.Input, 1).Execute().RegisterB()}";
    }
}
