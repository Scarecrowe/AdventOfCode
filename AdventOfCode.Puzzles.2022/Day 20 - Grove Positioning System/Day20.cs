namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_20___Grove_Positioning_System;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20()
        {
            this.DayTitle = "Grove Positioning System";
            this.GetPuzzleData(2, this.DayTitle);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new GrovePositioningSystem(this.Input).Cycle().Sum()}";

        [Slow]
        public string Gold() => $"{new GrovePositioningSystem(this.Input).Decrypt().Sum()}";
    }
}
