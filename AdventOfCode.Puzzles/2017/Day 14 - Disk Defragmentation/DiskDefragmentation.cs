namespace AdventOfCode.Puzzles._2017.Day_14___Disk_Defragmentation
{
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2017.Day_10___Knot_Hash;

    public class DiskDefragmentation
    {
        private static readonly Dictionary<char, string> Binary = new()
        {
            { '0', "0000" }, { '1', "0001" }, { '2', "0010" }, { '3', "0011" },
            { '4', "0100" }, { '5', "0101" }, { '6', "0110" }, { '7', "0111" },
            { '8', "1000" }, { '9', "1001" }, { 'a', "1010" }, { 'b', "1011" },
            { 'c', "1100" }, { 'd', "1101" }, { 'e', "1110" }, { 'f', "1111" }
        };

        private static readonly int Size = 128;

        public static int Squares(string input) => Parse(input).Join().Count(x => x == '1');

        public static int Regions(string input)
        {
            List<string> hashes = Parse(input);

            bool[,] visited = new bool[Size, Size];
            int regions = 0;

            for (int y = 0; y < visited.GetLength(1); y++)
            {
                for (int x = 0; x < visited.GetLength(0); x++)
                {
                    if (visited[x, y] || hashes[x][y] == '0')
                    {
                        continue;
                    }

                    Visit(x, y, hashes, visited);

                    regions++;
                }
            }

            return regions;
        }

        private static List<string> Parse(string input)
        {
            List<string> result = new();

            for (int i = 0; i < Size; i++)
            {
                result.Add(KnotHash.Hash($"{input}-{i}").Select(c => Binary[c]).Join());
            }

            return result;
        }

        private static void Visit(int x, int y, List<string> hashes, bool[,] visited)
        {
            if (visited[x, y] || hashes[x][y] == '0')
            {
                return;
            }

            visited[x, y] = true;

            if (x > 0)
            {
                Visit(x - 1, y, hashes, visited);
            }

            if (x < Size - 1)
            {
                Visit(x + 1, y, hashes, visited);
            }

            if (y > 0)
            {
                Visit(x, y - 1, hashes, visited);
            }

            if (y < Size - 1)
            {
                Visit(x, y + 1, hashes, visited);
            }
        }
    }
}
