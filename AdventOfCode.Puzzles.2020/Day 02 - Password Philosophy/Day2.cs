namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_02___Password_Philosophy;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2020, 2, "Password Philosophy")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{PasswordPhilosophy.Simple(this.Input)}";

        public string Gold() => $"{PasswordPhilosophy.Advanced(this.Input)}";
    }
}
