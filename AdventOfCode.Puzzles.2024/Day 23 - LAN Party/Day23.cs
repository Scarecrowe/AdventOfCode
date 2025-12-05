namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_23___LAN_Party;

    public class Day23 : Puzzle, IPuzzle
    {
        public Day23()
        {
            this.DayTitle = "LAN Party";
            this.GetPuzzleData(23, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new LANParty(this.Input).Interconnections()}";

        public string Gold() => $"{new LANParty(this.Input).Password()}";
    }
}
