namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_13___Claw_Contraption;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
            : base(2024, 13, "Claw Contraption", StringSplitOptions.None)
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ClawContraption(this.Input).FewestTokens()}";

        public string Gold() => $"{new ClawContraption(this.Input).FewestTokensWithOffset()}";
    }
}
