namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_07___Bridge_Repair;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
        {
            this.DayTitle = "Bridge Repair";
            this.GetPuzzleData(7, this.DayTitle);
        }

        public string Silver() => $"{new BridgeRepair(this.Input).Calibrate()}";

        public string Gold() => $"{new BridgeRepair(this.Input).Calibrate(true)}";
    }
}
