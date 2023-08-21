namespace AdventOfCode.Puzzles._2017.Day_04___High_Entropy_Passphrases
{
    using AdventOfCode.Core.Extensions;

    public class HighEntropyPassphrases
    {
        public static int Simple(string[] input) => input.Sum(x => !x.SplitSpace().GroupBy(x => x).Any(g => g.Count() > 1) ? 1 : 0);

        public static int Advanced(string[] input)
        {
            int result = 0;

            foreach (string line in input)
            {
                List<string> phrases = line.SplitSpace().ToList();

                for (int i = 0; i < phrases.Count; i++)
                {
                    phrases[i] = phrases[i].OrderBy(x => x).Join();
                }

                result += !phrases.GroupBy(x => x).Any(g => g.Count() > 1) ? 1 : 0;
            }

            return result;
        }
    }
}
