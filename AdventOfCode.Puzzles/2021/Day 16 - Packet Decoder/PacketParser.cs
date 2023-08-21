namespace AdventOfCode.Puzzles._2021.Day_16___Packet_Decoder
{
    using AdventOfCode.Core.Extensions;

    public class PacketParser
    {
        public PacketParser() => this.Decoded = new();

        private List<int> Decoded { get; set; }

        public List<Packet> Parse(string[] input)
        {
            List<Packet> result = new();
            this.Decoded = BinaryStream(input);
            int index = 0;

            while (index < (this.Decoded.Count - 11))
            {
                var versionOrType = this.VersionOrType(index);
                int version = versionOrType.Value;
                versionOrType = this.VersionOrType(versionOrType.Index);
                PacketType typeId = (PacketType)versionOrType.Value;

                var packetResult = versionOrType.Value == (int)PacketType.Literal
                    ? this.LiteralValue(version, versionOrType.Index)
                    : this.Operator(version, typeId, versionOrType.Index);

                index = packetResult.Index;
                result.Add(packetResult.Packet);
            }

            return result;
        }

        private static List<int> BinaryStream(string[] input)
        {
            List<int> result = new();

            foreach (char charA in input[0])
            {
                foreach (char charB in charA.ToString().ToInt(16).ToString(2).PadLeft(4, '0'))
                {
                    result.Add(charB.ToString().ToInt());
                }
            }

            return result;
        }

        private (Packet Packet, int Index) LiteralValue(int version, int index)
        {
            long value = 0;

            while (true)
            {
                string range = this.Decoded.GetRange(index, 5).Join();
                bool finished = range[0] == '0';
                value *= 16L;
                value += range.Remove(0, 1).ToInt(2);
                index += 5;

                if (finished)
                {
                    break;
                }
            }

            return (new(version, PacketType.Literal, value), index);
        }

        private (Packet Packet, int Index) Operator(int version, PacketType typeId, int index)
        {
            (int value, int index) result = this.LengthTypeId(index);
            index = result.index;

            int lengthTypeId = result.value;

            result = this.Length(index, result.value);
            index = result.index;

            Packet packet = new(version, typeId, 0);

            if (lengthTypeId == 0)
            {
                while (index < result.index + result.value)
                {
                    (int value, int index) subResult = this.VersionOrType(index);
                    index = subResult.index;

                    int subVersion = subResult.value;

                    subResult = this.VersionOrType(index);
                    index = subResult.index;

                    PacketType subTypeId = (PacketType)subResult.value;

                    (Packet? packet, int index) packetResult = subResult.value switch
                    {
                        4 => this.LiteralValue(subVersion, index),
                        _ => this.Operator(subVersion, subTypeId, index),
                    };

                    packet.AddSubPacket(packetResult.packet ?? new(0, PacketType.Sum, 0));

                    index = packetResult.index;
                }

                return (packet, index);
            }

            for (int i = 1; i <= result.value; i++)
            {
                (int value, int index) subResult = this.VersionOrType(index);
                index = subResult.index;

                int subVersion = subResult.value;

                subResult = this.VersionOrType(index);
                index = subResult.index;

                PacketType subTypeId = (PacketType)subResult.value;

                (Packet? packet, int index) packetResult = (null, 0);

                packetResult = subResult.value switch
                {
                    4 => this.LiteralValue(subVersion, index),
                    _ => this.Operator(subVersion, subTypeId, index),
                };

                packet.AddSubPacket(packetResult.packet ?? new(0, PacketType.Sum, 0));

                index = packetResult.index;
            }

            return (packet, index);
        }

        private (int Value, int Index) VersionOrType(int index) => (this.ToBinary(index, 3), index + 3);

        private (int Value, int Index) LengthTypeId(int index) => (this.ToBinary(index, 1), index + 1);

        private (int value, int index) Length(int index, int lengthTypeId)
        {
            int length = lengthTypeId == 0 ? 15 : 11;

            return (this.ToBinary(index, length), index + length);
        }

        private int ToBinary(int index, int count) => this.Decoded.GetRange(index, count).Join().ToInt(2);
    }
}
