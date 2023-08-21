namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_03___Spiral_Memory;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3(string file)
        {
            this.DayTitle = "Spiral Memory";
            this.GetPuzzleData(file);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{new SpiralMemory(int.Parse(this.Input[0])).Distance()}";

        public string Gold() => $"{new SpiralMemory(int.Parse(this.Input[0])).FillMemory()}";
    }
}
