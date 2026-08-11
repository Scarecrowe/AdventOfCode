namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_09___Stream_Processing;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
            : base(2017, 9, "Stream Processing")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new StreamProcessing(this.Input[0]).TotalScore}";

        public string Gold() => $"{new StreamProcessing(this.Input[0]).NonCanceled}";
    }
}
