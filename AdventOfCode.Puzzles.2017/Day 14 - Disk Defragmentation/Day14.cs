namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_14___Disk_Defragmentation;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
            : base(2017, 14, "Disk Defragmentation")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{DiskDefragmentation.Squares(this.Input[0])}";

        public string Gold() => $"{DiskDefragmentation.Regions(this.Input[0])}";
    }
}
