namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11(string file)
        {
            this.DayTitle = "Dumbo Octopus";
            this.GetPuzzleData(file);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new Cavern(this.Input).RunFor(100).Map.Flashes}";

        public string Gold() => $"{new Cavern(this.Input).RunUntil()}";
    }
}
