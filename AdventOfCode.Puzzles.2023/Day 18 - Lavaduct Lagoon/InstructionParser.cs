namespace AdventOfCode.Puzzles._2023.Day_18___Lavaduct_Lagoon
{
    using System.Globalization;

    public enum Direction { R, D, L, U }

    public static class InstructionParser
    {
        public static List<(Direction Dir, long Distance)> ParseNormalInstructions(string[] input)
        {
            var instructions = new List<(Direction Dir, long Distance)>();

            foreach (var line in input)
            {
                var parts = line.Split(new[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                var dir = Enum.Parse<Direction>(parts[0]);
                var dist = long.Parse(parts[1]);
                instructions.Add((dir, dist));
            }

            return instructions;
        }

        public static List<(Direction Dir, long Distance)> ParseHexInstructions(string[] input)
        {
            var instructions = new List<(Direction Dir, long Distance)>();

            foreach (var line in input)
            {
                int start = line.IndexOf('(');
                int end = line.IndexOf(')');
                string hex = line.Substring(start + 1, end - start - 1).TrimStart('#');

                long distance = long.Parse(hex.Substring(0, 5), NumberStyles.HexNumber);
                char dirHex = hex[5];
                Direction dir = dirHex switch
                {
                    '0' => Direction.R,
                    '1' => Direction.D,
                    '2' => Direction.L,
                    '3' => Direction.U,
                    _ => throw new ArgumentException($"Invalid direction hex: {dirHex}")
                };
                instructions.Add((dir, distance));
            }

            return instructions;
        }
    }
}

