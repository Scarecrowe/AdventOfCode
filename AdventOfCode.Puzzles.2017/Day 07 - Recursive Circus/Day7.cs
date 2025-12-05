namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_07___Recursive_Circus;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "Recursive Circus";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new RecursiveCircus(this.Input).BottomProgram()}";

        public string Gold() => $"{new RecursiveCircus(this.Input).BalancedWeight()}";
    }
}
