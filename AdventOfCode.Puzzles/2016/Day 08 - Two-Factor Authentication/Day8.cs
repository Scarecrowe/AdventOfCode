namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_08___Two_Factor_Authentication;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8(string file)
        {
            this.DayTitle = "Two-Factor Authentication";
            this.GetPuzzleData(file);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new TwoFactorAuthentication(this.Input).Pixels()}";

        public string Gold() => $"{new TwoFactorAuthentication(this.Input).Print()}";
    }
}
