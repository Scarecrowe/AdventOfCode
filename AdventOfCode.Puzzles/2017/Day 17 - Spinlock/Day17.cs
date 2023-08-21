namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2017.Day_17___Spinlock;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17(string file)
        {
            this.DayTitle = "Spinlock";
            this.GetPuzzleData(file);
        }

        public Day17(string[] input) => this.Input = input;

        public string Silver() => $"{SpinLock.Run(2017, this.Input[0].ToInt(), 2017)}";

        public string Gold() => $"{SpinLock.RunAngry(50000000, this.Input[0].ToInt())}";
    }
}
