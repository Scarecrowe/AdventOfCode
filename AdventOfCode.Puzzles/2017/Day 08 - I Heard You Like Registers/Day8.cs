namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_8___I_Heard_You_Like_Registers;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "I Heard You Like Registers";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new IHeardYouLikeRegisters(this.Input).Run().Max()}";

        public string Gold() => $"{new IHeardYouLikeRegisters(this.Input).Run().Size}";
    }
}
