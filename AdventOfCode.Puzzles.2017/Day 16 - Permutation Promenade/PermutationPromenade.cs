namespace AdventOfCode.Puzzles._2017.Day_16___Permutation_Promenade
{
    using System.Text;

    public class PermutationPromenade
    {
        public PermutationPromenade(string[] input) => this.Moves = Parse(input);

        public List<PromenadeMove> Moves { get; }

        public static string InitialValue()
        {
            StringBuilder sb = new();

            for (int i = (int)'a'; i <= (int)'p'; i++)
            {
                sb.Append((char)i);
            }

            return sb.ToString();
        }

        public string Sort(string value)
        {
            foreach (PromenadeMove move in this.Moves)
            {
                value = move.Run(value);
            }

            return value;
        }

        public string Sort(string value, int steps)
        {
            Dictionary<string, string> states = new();

            for (int i = 1; i <= steps; i++)
            {
                states.TryGetValue(value, out string? result);

                if (string.IsNullOrEmpty(result))
                {
                    string key = value;
                    value = this.Sort(value);

                    states.Add(key, value);

                    continue;
                }

                value = result;
            }

            return value;
        }

        private static List<PromenadeMove> Parse(string[] input)
        {
            List<PromenadeMove> result = new();

            string[] moves = input[0].Split(",");

            foreach (string move in moves)
            {
                result.Add(new(move));
            }

            return result;
        }
    }
}
