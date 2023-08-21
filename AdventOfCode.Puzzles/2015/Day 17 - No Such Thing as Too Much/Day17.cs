namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_17___No_Such_Thing_as_Too_Much;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "No Such Thing as Too Much";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{NoSuchThingAsTooMuch.ContainerCount(this.Input, 150)}";

        public string Gold() => $"{NoSuchThingAsTooMuch.CombinationCount(this.Input, 150)}";
    }
}
