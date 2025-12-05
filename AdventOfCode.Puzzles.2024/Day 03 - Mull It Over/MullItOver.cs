namespace AdventOfCode.Puzzles._2024.Day_03___Mull_It_Over
{
    using System.Text.RegularExpressions;

    public class MullItOver
    {
        public List<string> Operations { get; private set; }

        public (List<int> ValuesA, List<int> ValuesB) Values { get; private set; }

        public MullItOver(string[] input, bool flags) 
        {
            this.Operations = this.ParseOperations(input, flags);
            this.Values = GetValues();
        }

        public (List<int> ValuesA, List<int> ValuesB) GetValues()
        {
            List<int> resultA = [];
            List<int> resultB = [];

            foreach (string operation in this.Operations.Where(x => IsEquation(x)))
            {
                var (ValueA, ValueB) = GetValuesFromEquation(operation);
                resultA.Add(ValueA);
                resultB.Add(ValueB);
            }

            return (resultA, resultB);
        }

        private List<string> ParseOperations(string[] input, bool flags)
        {
            List<string> result = [];

            var pattern = !flags ? @"mul\(\d+,\d+\)" : @"mul\(\d+,\d+\)|do\(\)|don't\(\)";

            var matches = Regex.Matches(string.Join(string.Empty, input), pattern);

            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }

            if (!flags)
            {
                return result;
            }

            bool include = true;

            List<string> operations = [.. result];
            result.Clear();

            foreach (string operation in operations)
            {
                if (operation.StartsWith("mul(")
                    && operation.EndsWith(")")
                    && include)
                {
                    result.Add(operation);
                    continue;
                }

                if (operation == "don't()")
                {
                    include = false;
                }
                else if (operation == "do()")
                {
                    include = true;
                }
            }

            return result;
        }

        public static (List<int> ValuesA, List<int> ValuesB) GetEquations(List<string> equations)
        {
            List<int> resultA = [];
            List<int> resultB = [];

            foreach (var equation in equations)
            {
                if (IsEquation(equation))
                {
                    (int A, int B) = GetValuesFromEquation(equation);

                    resultA.Add(A);
                    resultB.Add(B);
                }
            }

            return (resultA, resultB);
        }

        public int Calculate()
        {
            var result = 0;

            for (var i = 0; i < this.Values.ValuesA.Count; i++)
            {
                result += this.Values.ValuesA[i] * this.Values.ValuesB[i];
            }

            return result;
        }

        private static bool IsEquation(string equation) => equation.StartsWith("mul(") && equation.EndsWith(")") && equation.Contains(",");

        private static (int ValueA, int ValueB) GetValuesFromEquation(string equation)
        {
            string values = equation[4..^1];
            string[] tokens = values.Split(',');

            return (int.Parse(tokens[0]), int.Parse(tokens[1]));
        }

    }
}
