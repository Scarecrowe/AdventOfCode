namespace AdventOfCode.Puzzles._2020.Day_18___Operation_Order
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class OperationOrder
    {
        public static long Simple(string[] input)
        {
            List<long> sums = new();

            foreach (string equation in input)
            {
                sums.Add(ParseEquationSimple(equation));
            }

            return sums.Sum();
        }

        public static long Advanced(string[] input)
        {
            List<long> sums = new();

            foreach (string equation in input)
            {
                sums.Add(ParseEquationAdvanced(equation));
            }

            return sums.Sum();
        }

        private static int FindClosingParentheses(string equation, int start)
        {
            int count = 1;

            for (int i = start + 1; i < equation.Length; i++)
            {
                switch (equation[i])
                {
                    case '(':
                        count++;
                        break;
                    case ')':
                        count--;

                        if (count == 0)
                        {
                            return i;
                        }

                        break;
                }
            }

            return 0;
        }

        private static long CalculateSimple(string equation)
        {
            Stack<string> operators = new();
            Stack<int> numbers = new();

            string[] equations = equation.SplitSpace();

            for (int i = 0; i < equations.Length; i++)
            {
                switch (equations[i])
                {
                    case "+":
                    case "*":
                        operators.Push(equations[i]);
                        break;
                    default:
                        numbers.Push(equations[i].ToInt());
                        break;
                }
            }

            operators = new(operators);
            numbers = new(numbers);

            long result = numbers.Pop();

            while (operators.Count > 0)
            {
                result = operators.Pop() == "+"
                    ? result + numbers.Pop()
                    : result * numbers.Pop();
            }

            return result;
        }

        private static long ParseEquationSimple(string equation)
        {
            string current = string.Empty;

            for (int i = 0; i < equation.Length; i++)
            {
                switch (equation[i])
                {
                    case '(':
                        current += ParseEquationSimple(equation[(i + 1) ..]);
                        i = FindClosingParentheses(equation, i);

                        break;
                    case ')':
                        return CalculateSimple(current);
                    default:
                        current += equation[i];
                        break;
                }
            }

            return CalculateSimple(current);
        }

        private static long CalculateAdvanced(string equation)
        {
            Stack<string> operators = new();
            Stack<long> numbers = new();

            string[] equations = equation.SplitSpace();

            for (int i = 0; i < equations.Length; i++)
            {
                switch (equations[i])
                {
                    case "+":
                    case "*":
                        operators.Push(equations[i]);
                        break;
                    default:
                        numbers.Push(equations[i].ToLong());
                        break;
                }
            }

            operators = new(operators);
            numbers = new(numbers);

            long result = numbers.Pop();

            while (operators.Count > 0)
            {
                result = operators.Pop() == "+"
                    ? result + numbers.Pop()
                    : result * numbers.Pop();
            }

            return result;
        }

        private static long ParseEquationAdvanced(string equation)
        {
            StringBuilder current = new();

            for (int i = 0; i < equation.Length; i++)
            {
                switch (equation[i])
                {
                    case '(':
                        current.Append(ParseEquationAdvanced(equation[(i + 1) ..]));
                        i = FindClosingParentheses(equation, i);

                        break;
                    case ')':
                        string tmp = Precedence(current.ToString());

                        if (tmp.Contains('('))
                        {
                            return ParseEquationAdvanced(tmp);
                        }

                        return CalculateAdvanced(tmp);
                    default:
                        current.Append(equation[i]);
                        break;
                }
            }

            string precedence = Precedence(current.ToString());

            if (precedence.Contains('('))
            {
                return ParseEquationAdvanced(precedence);
            }

            return CalculateAdvanced(precedence);
        }

        private static string Precedence(string equation)
        {
            if (equation.Contains('('))
            {
                return equation;
            }

            if (!(equation.Contains('+') && equation.Contains('*')))
            {
                return equation;
            }

            StringBuilder result = new();

            equation = equation.Replace(" ");

            bool closed = true;

            string number = string.Empty;

            for (int i = 0; i < equation.Length; i++)
            {
                if (i < equation.Length - 1
                    && equation[i + 1] == '+'
                    && closed)
                {
                    result.Append('(');
                    closed = false;
                }

                if (char.IsNumber(equation[i]))
                {
                    number += equation[i];
                }
                else
                {
                    result.Append(number);
                    number = string.Empty;
                    result.Append(equation[i]);
                }

                if (i < equation.Length - 1
                    && !closed
                    && equation[i + 1] == '*')
                {
                    result.Append(number);
                    number = string.Empty;
                    result.Append(')');
                    closed = true;
                }
            }

            result.Append(number);

            if (!closed)
            {
                result.Append(')');
            }

            return result.ToString().Replace("+", " + ").Replace("*", " * ");
        }
    }
}
