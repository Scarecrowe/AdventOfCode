namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_13___Transparent_Origami;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13(string file)
        {
            this.DayTitle = "Transparent Origami";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new TransparentOrigami(this.Input).Fold().Folds.ElementAt(0).Dots}";

        public string Gold() => $"{new TransparentOrigami(this.Input).Fold().Print()}";
    }
}
