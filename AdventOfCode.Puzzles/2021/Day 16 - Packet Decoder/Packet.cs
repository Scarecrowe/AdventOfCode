namespace AdventOfCode.Puzzles._2021.Day_16___Packet_Decoder
{
    public class Packet
    {
        public Packet(int version, PacketType typeId, long value)
        {
            this.Packets = new();
            this.Version = version;
            this.TypeId = typeId;
            this.Value = value;
        }

        public int Version { get; }

        public PacketType TypeId { get; }

        public long Value { get; }

        public List<Packet> Packets { get; private set; }

        public void AddSubPacket(Packet packet) => this.Packets.Add(packet);

        public List<long> Run(Packet packet)
        {
            List<long> result = new();

            switch (packet.TypeId)
            {
                case PacketType.Sum:
                    result.Add(packet.Sum());
                    break;
                case PacketType.Product:
                    result.Add(packet.Product());
                    break;
                case PacketType.Min:
                    result.Add(packet.Min());
                    break;
                case PacketType.Max:
                    result.Add(packet.Max());
                    break;
                case PacketType.Literal:
                    result.Add(packet.Value);
                    break;
                case PacketType.Greater:
                    result.Add(packet.Greater());
                    break;
                case PacketType.Less:
                    result.Add(packet.Less());
                    break;
                case PacketType.Equal:
                    result.Add(packet.Equal());
                    break;
            }

            return result;
        }

        public long Sum()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results.Sum();
        }

        public long Product()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results.Aggregate(1L, (a, b) => a * b);
        }

        public long Min()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results.Min();
        }

        public long Max()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results.Max();
        }

        public int Greater()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results[0] > results[1] ? 1 : 0;
        }

        public int Less()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results[0] < results[1] ? 1 : 0;
        }

        public long Equal()
        {
            List<long> results = new();

            foreach (Packet packet in this.Packets)
            {
                results.AddRange(this.Run(packet));
            }

            return results[0] == results[1] ? 1 : 0;
        }
    }
}
