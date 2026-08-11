namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_10___Elves_Look__Elves_Say;

    public class Day10 : Puzzle, IPuzzle
    {
        public Day10()
            : base(2015, 10, "Elves Look, Elves Say")
        {
        }

        public Day10(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{ElvesLookElvesSay.Play(this.Input[0], 40).Length}";

        public string Gold() => $"{ElvesLookElvesSay.Play(this.Input[0], 50).Length}";
    }
}
