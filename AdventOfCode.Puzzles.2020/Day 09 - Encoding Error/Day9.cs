namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_09___Encoding_Error;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
        {
            this.DayTitle = "Encoding Error";
            this.GetPuzzleData(9, this.DayTitle);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new EncodingError(this.Input).FindInvalid()}";

        public string Gold() => $"{new EncodingError(this.Input).FindWeakness()}";
    }
}
