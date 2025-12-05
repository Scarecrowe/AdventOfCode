namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_13___Claw_Contraption;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
        {
            this.DayTitle = "Claw Contraption";
            this.GetPuzzleData(13, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ClawContraption(this.Input).FewestTokens()}";

        public string Gold() => $"{new ClawContraption(this.Input).FewestTokensWithOffset()}";
    }
}
