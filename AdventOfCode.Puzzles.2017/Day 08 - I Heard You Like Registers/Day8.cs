namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_08___I_Heard_You_Like_Registers;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
            : base(2017, 8, "I Heard You Like Registers")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new IHeardYouLikeRegisters(this.Input).Run().Max()}";

        public string Gold() => $"{new IHeardYouLikeRegisters(this.Input).Run().Size}";
    }
}
