namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_10___Adapter_Array;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10(string file)
        {
            this.DayTitle = "Adapter Array";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day10(string[] input) => this.Input = input;

        public string Silver() => $"{AdapterArray.JoltDifference(this.Input)}";

        public string Gold() => $"{AdapterArray.Distinct(this.Input)}";
    }
}
