namespace AdventOfCode.Puzzles._2022.Day_21___Monkey_Math
{
    using System.Data;

    public class MonkeyMath
    {
        public MonkeyMath(string[] input) => this.Monkeys = ParseMonkeys(input);

        public Dictionary<string, Monkey> Monkeys { get; private set; }

        public long Root() => (long)this.Yell(this.Monkeys["root"]);

        public long RootEqual()
        {
            double low = 0;
            double high = 10000000000000;

            while (true)
            {
                this.Monkeys["humn"].Value = (long)Math.Floor((high + low) / 2);

                decimal valueA = this.Yell(this.Monkeys["root"].Left ?? new());
                decimal valueB = this.Yell(this.Monkeys["root"].Right ?? new());

                if (valueA == valueB)
                {
                    return this.Monkeys["humn"].Value;
                }

                if (valueA > valueB)
                {
                    low = this.Monkeys["humn"].Value;
                }
                else
                {
                    high = this.Monkeys["humn"].Value;
                }
            }
        }

        private static Dictionary<string, Monkey> ParseMonkeys(string[] input)
        {
            Dictionary<string, Monkey> result = new();

            foreach (string line in input)
            {
                string[] tokens = line.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens!.Length > 2)
                {
                    Monkey? current = result.TryGetValue(tokens[0], out current) ? current : new Monkey();
                    current.Left = result.TryGetValue(tokens[1], out Monkey? left) ? left : new Monkey();
                    current.Right = result.TryGetValue(tokens[3], out Monkey? right) ? right : new Monkey();
                    current.Operation = (MonkeyOperator)tokens[2][0];
                    result.TryAdd(tokens[0], current);
                    result.TryAdd(tokens[1], current.Left);
                    result.TryAdd(tokens[3], current.Right);
                }
                else
                {
                    Monkey? monkey = result.TryGetValue(tokens[0], out monkey) ? monkey : new Monkey();
                    monkey.Value = int.Parse(tokens[1]);
                    result.TryAdd(tokens[0], monkey);
                }
            }

            return result;
        }

        private string Equation(Monkey monkey)
        {
            if (monkey.Operation == MonkeyOperator.NONE)
            {
                return $"{monkey.Value}.0";
            }

            return $"({this.Equation(monkey.Left ?? new())} {(char)monkey.Operation} {this.Equation(monkey.Right ?? new())})";
        }

        private decimal Yell(Monkey monkey) => (decimal)new DataTable().Compute(this.Equation(monkey), string.Empty);
    }
}
