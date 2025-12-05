namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_03___Lobby;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
        {
            this.DayTitle = "Lobby";
            this.GetPuzzleData(3, this.DayTitle);
        }

        public string Silver() => $"{new Lobby(this.Input).Joltage(2)}";

        public string Gold() => $"{new Lobby(this.Input).Joltage(12)}";
    }
}
