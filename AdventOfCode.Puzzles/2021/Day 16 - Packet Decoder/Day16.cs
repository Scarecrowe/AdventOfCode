namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_16___Packet_Decoder;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Packet Decoder";
            this.GetPuzzleData(file);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new PacketDecoder(this.Input).Sum()}";

        public string Gold() => $"{new PacketDecoder(this.Input).Run()}";
    }
}
