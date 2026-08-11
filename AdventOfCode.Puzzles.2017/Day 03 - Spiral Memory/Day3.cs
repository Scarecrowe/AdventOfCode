namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_03___Spiral_Memory;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2017, 3, "Spiral Memory")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SpiralMemory(int.Parse(this.Input[0])).Distance()}";

        public string Gold() => $"{new SpiralMemory(int.Parse(this.Input[0])).FillMemory()}";
    }
}
