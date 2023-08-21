namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_14___One_Time_Pad;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14(string file)
        {
            this.DayTitle = "One-Time Pad";
            this.GetPuzzleData(file);
        }

        public Day14(string[] input) => this.Input = input;

        public string Silver() => $"{new OneTimePad(this.Input[0]).Generate()}";

        [Slow]
        public string Gold() => $"{new OneTimePad(this.Input[0]).Generate(true)}";
    }
}
