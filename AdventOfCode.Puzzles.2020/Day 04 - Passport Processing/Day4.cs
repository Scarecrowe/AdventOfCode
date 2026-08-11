namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_04___Passport_Processing;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
            : base(2020, 4, "Passport Processing", StringSplitOptions.None)
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{PassportProcessing.Simple(this.Input)}";

        public string Gold() => $"{PassportProcessing.Advanced(this.Input)}";
    }
}
