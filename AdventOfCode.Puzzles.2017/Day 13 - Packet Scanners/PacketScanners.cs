namespace AdventOfCode.Puzzles._2017.Day_13___Packet_Scanners
{
    using AdventOfCode.Core.Extensions;

    public class PacketScanners
    {
        public PacketScanners(string[] input) => this.Layers = Parse(input);

        private List<(int Layer, int Depth)> Layers { get; }

        public int Severity(int? delay = null)
        {
            int result = 0;

            foreach (var (layer, depth) in this.Layers)
            {
                if (IsCaught(layer + (delay ?? 0), depth))
                {
                    result += layer * depth;

                    if (delay.HasValue)
                    {
                        return -1;
                    }
                }
            }

            return result;
        }

        public int Picoseconds()
        {
            int result = 1;

            while (result > 0)
            {
                if (this.Severity(result) == 0)
                {
                    return result;
                }

                result++;
            }

            throw new InvalidOperationException();
        }

        private static List<(int Layer, int Depth)> Parse(string[] input)
        {
            List<(int Layer, int Depth)> result = new();

            foreach (var line in input)
            {
                int[] pair = line.Split(": ").ToInt();
                result.Add((pair[0], pair[1]));
            }

            return result;
        }

        private static bool IsCaught(int layer, int depth)
        {
            int cycle = 2 * (depth - 1);

            return (layer % cycle) == 0;
        }
    }
}
