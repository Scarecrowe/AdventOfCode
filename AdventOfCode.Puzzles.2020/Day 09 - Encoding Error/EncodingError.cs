namespace AdventOfCode.Puzzles._2020.Day_09___Encoding_Error
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class EncodingError
    {
        public EncodingError(string[] input) => this.Input = input;

        private string[] Input { get; }

        public int FindInvalid()
        {
            int preamble = 25;

            for (int i = preamble; i < this.Input.Length; i++)
            {
                int value = this.Input[i].ToInt();
                bool found = false;

                for (int j = i - preamble; j < i; j++)
                {
                    int current = this.Input[j].ToInt();

                    for (int k = i - preamble; k < i; k++)
                    {
                        int next = this.Input[k].ToInt();

                        if (next != current && next + current == value)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }

                if (!found)
                {
                    return value;
                }
            }

            return -1;
        }

        public int FindWeakness()
        {
            int invalid = this.FindInvalid();

            for (int i = 0; i < this.Input.Length; i++)
            {
                List<int> values = new() { this.Input[i].ToInt() };

                for (int j = i + 1; j < this.Input.Length; j++)
                {
                    values.Add(this.Input[j].ToInt());

                    int sum = values.Sum();

                    if (sum > invalid)
                    {
                        break;
                    }

                    if (sum == invalid)
                    {
                        return values.Min() + values.Max();
                    }
                }
            }

            return -1;
        }
    }
}
