namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_08___Space_Image_Format;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Space Image Format";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new SpaceImageFormat(this.Input[0], 25, 6).FewestDigits()}";

        public string Gold() => $"{new SpaceImageFormat(this.Input[0], 25, 6).Decode().Print()}";
    }
}
