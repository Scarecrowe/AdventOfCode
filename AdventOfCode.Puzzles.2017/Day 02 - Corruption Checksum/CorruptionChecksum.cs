namespace AdventOfCode.Puzzles._2017.Day_02___Corruption_Checksum
{
    using AdventOfCode.Core.Extensions;

    public class CorruptionChecksum
    {
        public CorruptionChecksum(string[] input) => this.Input = input.Select(x => x.Split('\t').ToIntList()).ToList();

        public List<List<int>> Input { get; }

        public long SumOfLargestSmallest() => this.Input.Sum(x => x.Max() - x.Min());

        public long SumOfEven()
        {
            int result = 0;

            foreach (List<int> numbers in this.Input)
            {
                foreach (int numberA in numbers)
                {
                    foreach (int numberB in numbers)
                    {
                        if (numberA == numberB)
                        {
                            continue;
                        }

                        if (numberA % numberB == 0)
                        {
                            result += numberA / numberB;
                        }
                    }
                }
            }

            return result;
        }
    }
}
