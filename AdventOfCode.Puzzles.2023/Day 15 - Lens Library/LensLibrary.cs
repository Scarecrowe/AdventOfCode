namespace AdventOfCode.Puzzles._2023.Day_15___Lens_Library
{
    using System.Collections.Specialized;

    public class LensLibrary
    {
        public LensLibrary(string[] input)
        {
            this.InstructionHashSum = input[0].Split(",").Aggregate(0, (result, next) => result += Hash(next));
            this.Boxes = ParseBoxes(input);
        }

        public Dictionary<int, OrderedDictionary> Boxes { get; }

        public int InstructionHashSum { get; }

        public int FocusingPower()
        {
            int result = 0;

            foreach (var box in this.Boxes)
            {
                for (int i = 0; i < box.Value.Count; i++)
                {
                    result += (1 + box.Key) * (i + 1) * (int)(box.Value[i] ?? 0);
                }
            }

            return result;
        }

        private static int Hash(string value)
            => value.Aggregate(0, (result, next) =>
            {
                result += (int)next;
                result *= 17;
                result %= 256;

                return result;
            });

        private static IEnumerable<(string, int, int, char)> InstructionEnumerator(string[] input)
        {
            foreach(string instruction in input[0].Split(","))
            {
                string label = new(instruction.Where(x => char.IsLetter(x)).ToArray());
                int box = Hash(label);
                int length = instruction.EndsWith("-")
                    ? 0
                    : int.Parse(instruction[(instruction.LastIndexOf('=') + 1) ..]);

                yield return (label, box, length, length == 0 ? '-' : '=');
            }
        }

        private static Dictionary<int, OrderedDictionary> ParseBoxes(string[] input)
        {
            Dictionary<int, OrderedDictionary> result = new();

            foreach (var (label, box, length, instruction) in InstructionEnumerator(input))
            {
                if (instruction == '-')
                {
                    if (result.ContainsKey(box))
                    {
                        result[box].Remove(label);

                        if (result[box].Count == 0)
                        {
                            result.Remove(box);
                        }
                    }

                    continue;
                }

                if (!result.ContainsKey(box))
                {
                    result.Add(box, new() { { label, length } });
                    continue;
                }

                if (result[box].Contains(label))
                {
                    result[box][label] = length;
                    continue;
                }

                result[box].Add(label, length);
            }

            return result;
        }
    }
}
