namespace AdventOfCode.Puzzles._2017.Day_10___Knot_Hash
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class KnotHash
    {
        public static int Product(string input)
        {
            var hash = input
            .Split(',')
            .ToByte()
            .HashSequence(1)
            .Last();

            return hash[0] * hash[1];
        }

        public static string Hash(string input)
        {
            var hash = input
            .ToCharArray()
            .Select(i => (byte)i)
            .Concat(new byte[] { 0x11, 0x1f, 0x49, 0x2f, 0x17 })
            .HashSequence(64)
            .Last();

            IEnumerable<int>? denseHash = hash
                .Select((v, i) => (value: v, index: i))
                .GroupBy(i => i.index / 16)
                .Select(g => g.Aggregate(0x0, (acc, i) => (byte)(acc ^ i.value)));

            return denseHash
                .Aggregate(new StringBuilder(), (acc, i) => acc.Append($"{i:x2}"))
                .ToString();
        }
    }
}
