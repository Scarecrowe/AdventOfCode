namespace AdventOfCode.Puzzles._2016.Day_21___Scrambled_Letters_and_Hash
{
    using AdventOfCode.Core.Extensions;

    public class ScrambledLettersAndHash
    {
        public ScrambledLettersAndHash(string[] input) => this.Input = input;

        private string[] Input { get; set; }

        public string Unscramble(string value)
        {
            List<List<char>> permutations = value.ToCharArray().ToList().Permutations(value.Length);

            foreach (List<char> permutation in permutations)
            {
                string current = permutation.Join();

                if (this.Scramble(current) == value)
                {
                    return current;
                }
            }

            return string.Empty;
        }

        public string Scramble(string value)
        {
            foreach (string line in this.Input)
            {
                if (line.StartsWith("swap p"))
                {
                    int[] numbers = line.Replace("swap position ")
                        .Split(" with position ")
                        .ToInt();

                    value = value.SwapPosition(numbers[0], numbers[1]);
                }
                else if (line.StartsWith("swap l"))
                {
                    char[] chars = line.Replace("swap letter ")
                        .Split(" with letter ")
                        .Select(x => x[0])
                        .ToArray();

                    value = value.SwapLetter(chars[0], chars[1]);
                }
                else if (line.StartsWith("rotate b"))
                {
                    value = value.RotateAroundChar(line.Last());
                }
                else if (line.StartsWith("rotate r"))
                {
                    value = value.RotateRight(line.Replace("rotate right ").Replace(" steps").Replace(" step").ToInt());
                }
                else if (line.StartsWith("reverse"))
                {
                    int[] numbers = line.Replace("reverse positions ")
                        .Split(" through ")
                        .ToInt();

                    value = value.Reverse(numbers[0], numbers[1]);
                }
                else if (line.StartsWith("rotate l"))
                {
                    value = value.RotateLeft(line.Replace("rotate left ").Replace(" steps").Replace(" step").ToInt());
                }
                else
                {
                    int[] numbers = line.Replace("move position ").Split(" to position ").ToInt();

                    value = value.Move(numbers[0], numbers[1]);
                }
            }

            return value;
        }
    }
}