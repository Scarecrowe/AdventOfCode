namespace AdventOfCode.Puzzles._2022.Day_04___Camp_Cleanup
{
    using AdventOfCode.Core.Extensions;

    public class CampCleanup
    {
        public static int Contains(string[] input)
        {
            List<string> results = new();

            foreach (string line in input)
            {
                string[] tokens = line.Split(",");

                foreach (string token in tokens)
                {
                    int[] sections = token.Split("-").ToInt();

                    List<string> sectionIds = new();

                    for (int i = sections[0]; i <= sections[1]; i++)
                    {
                        sectionIds.Add($"[{i}]");
                    }

                    results.Add(sectionIds.Join());
                }
            }

            int count = 0;

            for (int i = 0; i < results.Count; i += 2)
            {
                if (results[i].Length > results[i + 1].Length)
                {
                    if (results[i].IndexOf(results[i + 1]) > -1)
                    {
                        count++;
                    }
                }
                else
                {
                    if (results[i + 1].IndexOf(results[i]) > -1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int Overlap(string[] input)
        {
            List<string> results = new();

            foreach (string line in input)
            {
                string[] tokens = line.Split(",");

                foreach (string token in tokens)
                {
                    int[] sections = token.Split("-").ToInt();

                    List<string> sectionIds = new();

                    for (int i = sections[0]; i <= sections[1]; i++)
                    {
                        sectionIds.Add($"[{i}]");
                    }

                    results.Add(sectionIds.Join());
                }
            }

            int count = 0;

            for (int i = 0; i < results.Count; i += 2)
            {
                string[] numbers = results[i].Split("]", StringSplitOptions.RemoveEmptyEntries);

                foreach (string number in numbers)
                {
                    if (results[i + 1].IndexOf($"{number}]") > -1)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }
    }
}
