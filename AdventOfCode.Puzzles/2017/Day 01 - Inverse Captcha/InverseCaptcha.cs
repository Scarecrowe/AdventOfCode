namespace AdventOfCode.Puzzles._2017.Day_01___Inverse_Captcha
{
    using AdventOfCode.Core.Extensions;

    public class InverseCaptcha
    {
        public static int Simple(string input)
        {
            List<int> numbers = new();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[i + 1 == input.Length ? 0 : i + 1])
                {
                    numbers.Add(input[i].ToString().ToInt());
                }
            }

            return numbers.Sum();
        }

        public static int Advanced(string input)
        {
            List<int> numbers = new();

            int steps = input.Length / 2;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == input[(i + steps) % input.Length])
                {
                    numbers.Add(input[i].ToString().ToInt());
                }
            }

            return numbers.Sum();
        }
    }
}
