namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7(string file)
        {
            this.DayTitle = "The Sum of Its Parts";
            this.GetPuzzleData(file);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new TheSumOfItsParts(this.Input).AssembleyOrder()}";

        public string Gold() => $"{new TheSumOfItsParts(this.Input).AssemblyTime(5)}";
    }
}
