namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_15___Dueling_Generators;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Dueling Generators";
            this.GetPuzzleData(file);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new DuelingGenerators(this.Input).Simple()}";

        public string Gold() => $"{new DuelingGenerators(this.Input).Advanced()}";
    }
}
