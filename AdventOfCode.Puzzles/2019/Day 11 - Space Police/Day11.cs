namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_11___Space_Police;

    public class Day11 : Puzzle, IPuzzle
    {
        public Day11(string file)
        {
            this.DayTitle = "Space Police";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day11(string[] input) => this.Input = input;

        public string Silver() => $"{new SpacePolice(this.Input[0]).PaintHull(0, 0, PaintColour.Black).Hull.Count}";

        public string Gold() => $"{new SpacePolice(this.Input[0]).PaintHull(0, 1, PaintColour.White).Print()}";
    }
}
