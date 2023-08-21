namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_16___Aunt_Sue;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Aunt Sue";
            this.GetPuzzleData(file);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new AuntSue(this.Input).FindExactSue()}";

        public string Gold() => $"{new AuntSue(this.Input).FindSue()}";
    }
}
