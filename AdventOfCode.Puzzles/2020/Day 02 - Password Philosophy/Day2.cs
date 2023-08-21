namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_2___Password_Philosophy;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Password Philosophy";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{PasswordPhilosophy.Simple(this.Input)}";

        public string Gold() => $"{PasswordPhilosophy.Advanced(this.Input)}";
    }
}
