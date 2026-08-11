namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_23___LAN_Party;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
            : base(2024, 23, "LAN Party" , StringSplitOptions.None)
        {
        }

        public Day23(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new LANParty(this.Input).Interconnections()}";

        public string Gold() => $"{new LANParty(this.Input).Password()}";
    }
}
