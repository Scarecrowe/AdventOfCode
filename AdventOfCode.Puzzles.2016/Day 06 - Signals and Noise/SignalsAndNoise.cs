namespace AdventOfCode.Puzzles._2016.Day_06___Signals_and_Noise
{
    using AdventOfCode.Core.Extensions;

    public class SignalsAndNoise
    {
        public static string ValidMessage(string[] input, bool descending = true)
        {
            Dictionary<int, Dictionary<char, int>> messages = new();

            for (int i = 0; i < input[0].Length; i++)
            {
                messages.Add(i, new());
            }

            foreach (string message in input)
            {
                for (int i = 0; i < message.Length; i++)
                {
                    char character = message[i];

                    if (!messages[i].ContainsKey(character))
                    {
                        messages[i].Add(character, 1);

                        continue;
                    }

                    messages[i][character]++;
                }
            }

            return descending
                ? messages.Select(x => x.Value.OrderByDescending(y => y.Value).First().Key).Join()
                : messages.Select(x => x.Value.OrderBy(y => y.Value).First().Key).Join();
        }
    }
}
