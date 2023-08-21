namespace AdventOfCode.Puzzles._2017.Day_21___Fractal_Art
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class FractalArt
    {
        public FractalArt(string[] input)
        {
            this.Rules = ParseRules(input);
            this.Pattern = new(".#./..#/###".Split("/"));
        }

        public Dictionary<string, int[,]> Rules { get; }

        public Pattern Pattern { get; }

        public static string CreateKey(int[,] grid)
        {
            List<string> rows = new();
            StringBuilder sb = new();

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    sb.Append(grid[y, x]);
                }

                rows.Add(sb.ToString());
                sb.Clear();
            }

            return rows.Join("/");
        }

        public FractalArt Enhance(int iterations)
        {
            Enumerable.Range(1, iterations).ForEach(x => this.Pattern.Enhance(this.Rules));

            return this;
        }

        private static Dictionary<string, int[,]> ParseRules(string[] input)
        {
            Dictionary<string, int[,]> result = new();

            foreach (string line in input)
            {
                string[] tokens = line.Split(" => ");

                int[,] output = ParseRule(tokens[1].Split("/"));

                result.TryAddValue(tokens[0], output);

                int[,] rule = ParseRule(tokens[0].Split("/"));

                int size = rule.GetLength(0);

                result.TryAddValue(CreateKey(rule.FlipHorizontally(size)), output);

                int[,] rotated = rule.RotateClockWise(size);

                for (int i = 0; i < 3; i++)
                {
                    result.TryAddValue(CreateKey(rotated), output);
                    result.TryAddValue(CreateKey(rotated.FlipHorizontally(size)), output);

                    rotated = rotated.RotateClockWise(size);
                }
            }

            return result;
        }

        private static int[,] ParseRule(string[] rule)
        {
            int[,] result = new int[rule.Length, rule[0].Length];

            for (int y = 0; y < rule.Length; y++)
            {
                for (int x = 0; x < rule[0].Length; x++)
                {
                    result[y, x] = rule[y][x] == '#' ? 1 : 0;
                }
            }

            return result;
        }
    }
}
