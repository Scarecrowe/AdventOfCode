namespace AdventOfCode.Puzzles._2021.Day_24___Arithmetic_Logic_Unit
{
    using AdventOfCode.Core.Extensions;

    public class ArithmeticLogicUnit
    {
        public ArithmeticLogicUnit(string[] input) => this.Pairs = Parse(input);

        private List<(int A, int B)> Pairs { get; }

        public long Largest() => this.Run(new((y) => Math.Min(9, 9 + y)), new((y) => Math.Min(9, 9 - y)));

        public long Smallest() => this.Run(new((y) => Math.Max(1, 1 + y)), new((y) => Math.Max(1, 1 - y)));

        private static List<(int A, int B)> Parse(string[] input)
        {
            List<(int A, int B)> result = new();

            for (int i = 0; i < 14; i++)
            {
                result.Add((input[(i * 18) + 5][6..].ToInt(), input[(i * 18) + 15][6..].ToInt()));
            }

            return result;
        }

        private long Run(Func<int, int> keyResult, Func<int, int> xResult)
        {
            Stack<(int A, int B)> stack = new();
            Dictionary<int, (int x, int y)> keys = new();

            foreach (var (pair, i) in this.Pairs.Select((pair, i) => (pair, i)))
            {
                if (pair.A > 0)
                {
                    stack.Push((i, pair.B));
                }
                else
                {
                    var (j, addr) = stack.Pop();
                    keys[i] = (j, addr + pair.A);
                }
            }

            Dictionary<int, int> result = new();

            foreach (var (key, value) in keys)
            {
                result[key] = keyResult(value.y);
                result[value.x] = xResult(value.y);
            }

            return result.OrderBy(x => x.Key).Select(x => x.Value).Join().ToLong();
        }
    }
}
