namespace AdventOfCode.Puzzles._2015.Day_05___Doesn_t_He_Have_Intern_Elves_For_This
{
    public class DoesntHeHaveInternElvesForThis
    {
        public DoesntHeHaveInternElvesForThis(string[] input, ChristmasListModel model)
        {
            foreach (string line in input)
            {
                if (model == ChristmasListModel.Normal
                        ? IsNiceNormal(line)
                        : IsNiceAdvanced(line))
                {
                    this.Nice++;
                }
                else
                {
                    this.Naughty++;
                }
            }
        }

        public int Naughty { get; }

        public int Nice { get; }

        private static HashSet<char> Vowels { get; } = new() { 'a', 'e', 'i', 'o', 'u' };

        private static HashSet<string> Disallowed { get; } = new() { "ab", "cd", "pq", "xy" };

        private static bool IsNiceNormal(string line)
        {
            char last = '\0';
            bool repeat = false;
            bool disallowed = false;
            int vowels = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == last)
                {
                    repeat = true;
                }

                if (Vowels.Contains(line[i]))
                {
                    vowels++;
                }

                if (Disallowed.Contains($"{last}{line[i]}"))
                {
                    disallowed = true;
                    break;
                }

                last = line[i];
            }

            return !disallowed && vowels >= 3 && repeat;
        }

        private static bool IsNiceAdvanced(string line)
        {
            Dictionary<string, (int Index, int Count)> pairs = new();
            bool repeat = false;

            for (int i = 0; i < line.Length - 1; i++)
            {
                string pair = $"{line[i]}{line[i + 1]}";

                if (!pairs.ContainsKey(pair))
                {
                    pairs.Add(pair, (i, 1));
                }
                else
                {
                    (int Index, int Count) value = pairs[pair];

                    if (i > value.Index + 1)
                    {
                        pairs[pair] = (value.Index, value.Count + 1);
                    }
                }

                if (i > 0 && line[i - 1] == line[i + 1])
                {
                    repeat = true;
                }
            }

            return pairs.Values.Select(x => x.Count).Max() >= 2 && repeat;
        }
    }
}
