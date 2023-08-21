namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_06___Tuning_Trouble;

    public class Day6 : Puzzle, IPuzzle
    {
        public Day6(string file)
        {
            this.DayTitle = "Tuning Trouble";
            this.GetPuzzleData(file);
        }

        public Day6(string[] input) => this.Input = input;

        public string Silver() => $"{TuningTrouble.Marker(this.Input[0], 4)}";

        public string Gold() => $"{TuningTrouble.Marker(this.Input[0], 14)}";
    }
}
