namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_07___Recursive_Circus;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7(string file)
        {
            this.DayTitle = "Recursive Circus";
            this.GetPuzzleData(file);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new RecursiveCircus(this.Input).BottomProgram()}";

        public string Gold() => $"{new RecursiveCircus(this.Input).BalancedWeight()}";
    }
}
