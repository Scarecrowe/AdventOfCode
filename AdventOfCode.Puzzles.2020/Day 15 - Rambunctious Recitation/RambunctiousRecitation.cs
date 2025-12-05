namespace AdventOfCode.Puzzles._2020.Day_15___Rambunctious_Recitation
{
    using AdventOfCode.Core.Extensions;

    public class RambunctiousRecitation
    {
        public RambunctiousRecitation(string[] input) => this.Input = input;

        public string[] Input { get; }

        public int LastSpoken(int max)
        {
            int[] numbers = this.Input[0].Split(",").ToInt();
            Dictionary<int, List<int>> spoken = new();
            int index;

            for (index = 1; index <= numbers.Length; index++)
            {
                spoken.Add(numbers[index - 1], new() { index });
            }

            if (numbers[0] != 0)
            {
                spoken.Add(0, new() { index });
                index++;
                spoken[0].Add(index);
            }
            else
            {
                spoken[0].Add(index);
            }

            index++;

            int lastSpoken = 0;
            for (int i = index; i <= max; i++)
            {
                lastSpoken = spoken[lastSpoken][^1] - spoken[lastSpoken][^2];

                if (spoken.ContainsKey(lastSpoken))
                {
                    spoken[lastSpoken].Add(i);
                }
                else
                {
                    spoken.Add(lastSpoken, new() { i });

                    if (i == max)
                    {
                        return lastSpoken;
                    }

                    lastSpoken = 0;
                    i++;
                    spoken[lastSpoken].Add(i);
                }
            }

            return lastSpoken;
        }
    }
}
