namespace AdventOfCode.Puzzles._2019.Day_04___Secure_Container
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SecureContainer
    {
        public static int Simple(string[] input)
        {
            Vector<int> range = Range(input);
            List<string> values = new();
            List<string> first = new();
            List<string> second = new();

            for (int i = range.X; i <= range.Y; i++)
            {
                if ($"{i}" == $"{i}".OrderBy(c => c).Join())
                {
                    first.Add($"{i}");
                }
            }

            int count = 0;

            foreach (string number in first)
            {
                foreach (char digit in number)
                {
                    count = number.Split(digit).Length - 1;

                    if (count >= 2)
                    {
                        second.Add(number);
                        break;
                    }
                }
            }

            return second.Count;
        }

        public static int Advanced(string[] input)
        {
            Vector<int> range = Range(input);
            List<string> values = new();
            List<string> first = new();
            List<string> second = new();

            for (int i = range.X; i <= range.Y; i++)
            {
                if ($"{i}" == $"{i}".OrderBy(c => c).Join())
                {
                    first.Add($"{i}");
                }
            }

            int count = 0;

            foreach (string number in first)
            {
                foreach (char digit in number)
                {
                    count = number.Split(digit).Length - 1;

                    if (count == 2)
                    {
                        second.Add(number);
                        break;
                    }
                }
            }

            return second.Count;
        }

        private static Vector<int> Range(string[] input) => new(input[0].Split("-").ToInt());
    }
}
