namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_13___Packet_Scanners;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13()
        {
            this.DayTitle = "Packet Scanners";
            this.GetPuzzleData(13, this.DayTitle);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new PacketScanners(this.Input).Severity()}";

        public string Gold() => $"{new PacketScanners(this.Input).Picoseconds()}";
    }
}
