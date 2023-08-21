namespace AdventOfCode.Puzzles._2022.Day_05___Supply_Stacks
{
    using AdventOfCode.Core.Extensions;

    public class SupplyStacks
    {
        public static string Single(string[] input)
        {
            List<Stack<char>> crates = ParseStacks(input);

            foreach (int[] move in input.ToList().Split(string.Empty).ElementAt(1).Select(x => ParseMove(x)))
            {
                for (int i = 1; i <= move[0]; i++)
                {
                    char toMove = crates[move[1] - 1].Pop();

                    crates[move[2] - 1].Push(toMove);
                }
            }

            return crates.Select(x => x.Pop()).Join();
        }

        public static string Multiple(string[] input)
        {
            List<Stack<char>> crates = ParseStacks(input);

            foreach (int[] move in input.ToList().Split(string.Empty).ElementAt(1).Select(x => ParseMove(x)))
            {
                List<char> toMove = new();

                for (int i = 1; i <= move[0]; i++)
                {
                    toMove.Add(crates[move[1] - 1].Pop());
                }

                toMove.Reverse();

                foreach (char chr in toMove)
                {
                    crates[move[2] - 1].Push(chr);
                }
            }

            return crates.Select(x => x.Pop()).Join();
        }

        private static List<Stack<char>> ParseStacks(string[] input)
        {
            List<Stack<char>> crates = new();
            int index = 0;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        string[] tokens = input[i].SplitSpace();

                        if (index - 1 == i)
                        {
                            foreach (string token in tokens)
                            {
                                if (!string.IsNullOrEmpty(token))
                                {
                                    crates.Add(new());
                                }
                            }

                            continue;
                        }

                        int z = 0;

                        for (int y = 0; y < input[i].Length; y += 4)
                        {
                            string token = y + 4 >= input[i].Length ? input[i][y..].Trim() : input[i].Substring(y, 4).Trim();

                            if (!string.IsNullOrEmpty(token))
                            {
                                crates[z].Push(token.Replace("[").Replace("]")[0]);
                            }

                            z++;
                        }
                    }

                    break;
                }

                index++;
            }

            return crates;
        }

        private static int[] ParseMove(string line)
            => line.Replace("move ").Replace("from ").Replace("to ").SplitSpace().ToInt();
    }
}
