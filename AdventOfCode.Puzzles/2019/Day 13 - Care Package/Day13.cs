namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_13___Care_Package;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13(string file)
        {
            this.DayTitle = "Care Package";
            this.GetPuzzleData(file);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new CarePackage(this.Input[0]).Play().CountBlocks()}";

        public string Gold() => $"{new CarePackage(this.Input[0]).Play(2).Score}";
    }
}
