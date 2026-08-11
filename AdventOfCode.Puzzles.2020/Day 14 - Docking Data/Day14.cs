namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_14___Docking_Data;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
            : base(2020, 14, "Docking Data")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{DockingData.Version1(this.Input)}";

        public string Gold() => $"{DockingData.Version2(this.Input)}";
    }
}
