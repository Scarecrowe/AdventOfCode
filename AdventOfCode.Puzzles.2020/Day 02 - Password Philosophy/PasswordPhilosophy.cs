namespace AdventOfCode.Puzzles._2020.Day_2___Password_Philosophy
{
    using AdventOfCode.Core.Extensions;

    public class PasswordPhilosophy
    {
        public static int Simple(string[] input)
        {
            int valid = 0;

            foreach (string line in input)
            {
                string[] tokens = line.SplitSpace();
                string[] policy = tokens[0].Trim().Split("-");
                int min = policy[0].ToInt();
                int max = policy[1].ToInt();
                char letter = tokens[1].Trim().Replace(":")[0];
                string password = tokens[2].Trim();
                int count = password.Count(x => x == letter);

                if (count >= min && count <= max)
                {
                    valid++;
                }
            }

            return valid;
        }

        public static int Advanced(string[] input)
        {
            int valid = 0;

            foreach (string line in input)
            {
                string[] tokens = line.SplitSpace();
                string[] policy = tokens[0].Trim().Split("-");
                int min = policy[0].ToInt();
                int max = policy[1].ToInt();
                string letter = tokens[1].Trim().Replace(":");
                string password = tokens[2].Trim();
                bool minMatch = false;
                bool maxMatch = false;

                if (password[min - 1] == letter[0])
                {
                    minMatch = true;
                }

                if (password[max - 1] == letter[0])
                {
                    maxMatch = true;
                }

                if ((minMatch && !maxMatch) || (!minMatch && maxMatch))
                {
                    valid++;
                }
            }

            return valid;
        }
    }
}
