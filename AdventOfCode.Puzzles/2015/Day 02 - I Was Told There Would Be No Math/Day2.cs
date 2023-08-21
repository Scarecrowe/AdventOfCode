namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_02___I_Was_Told_There_Would_Be_No_Math;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "I Was Told There Would Be No Math";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{new IWasToldThereWouldBeNoMath(this.Input).WrappingPaper()}";

        public string Gold() => $"{new IWasToldThereWouldBeNoMath(this.Input).Ribbon()}";
    }
}
