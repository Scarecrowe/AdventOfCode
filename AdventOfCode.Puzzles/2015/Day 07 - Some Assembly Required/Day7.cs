namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_07___Some_Assembly_Required;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7(string file)
        {
            this.DayTitle = "ProbablySome Assembly Required";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day7(string[] input) => this.Input = input;

        public string Silver() => $"{new SomeAssemblyRequired(this.Input).Assemble().WireA()}";

        public string Gold() => $"{new SomeAssemblyRequired(this.Input).Assemble().Reassemble().WireA()}";
    }
}
