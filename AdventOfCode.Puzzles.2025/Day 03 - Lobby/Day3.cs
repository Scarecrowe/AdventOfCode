namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_03___Lobby;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2025, 3, "Lobby")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new Lobby(this.Input).Joltage(2)}";

        public string Gold() => $"{new Lobby(this.Input).Joltage(12)}";
    }
}
