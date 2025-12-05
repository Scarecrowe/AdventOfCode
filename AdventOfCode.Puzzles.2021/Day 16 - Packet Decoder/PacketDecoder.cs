namespace AdventOfCode.Puzzles._2021.Day_16___Packet_Decoder
{
    public class PacketDecoder
    {
        public PacketDecoder(string[] input) => this.Packets = new PacketParser().Parse(input);

        public List<Packet> Packets { get; }

        public long Run() => this.Packets[0].Run(this.Packets[0]).First();

        public long Sum(Packet? packet = null)
            => packet == null
                ? this.Packets.Sum(x => this.Sum(x))
                : packet.Packets.Sum(x => this.Sum(x)) + packet.Version;
    }
}
