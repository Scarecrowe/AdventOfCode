namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_15___Oxygen_System;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15()
        {
            this.DayTitle = "Oxygen System";
            this.GetPuzzleData(15, this.DayTitle);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new OxygenSystem(this.Input[0]).BuildMap().Distance}";

        public string Gold() => $"{new OxygenSystem(this.Input[0]).BuildMap().FillMap().Minutes}";
    }
}
