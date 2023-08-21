namespace AdventOfCode.Puzzles._2020.Day_10___Adapter_Array
{
    using AdventOfCode.Core.Extensions;

    public class AdapterArray
    {
        private static readonly Dictionary<int, long> Valid = new() { { 2, 1 }, { 3, 2 }, { 4, 4 }, { 5, 7 } };

        public static int JoltDifference(string[] input)
        {
            List<int> adaptors = input.ToIntList();
            adaptors.Sort();

            int joltage = 0;
            int diff1 = 0;
            int diff3 = 1;

            while (adaptors.Count > 0)
            {
                for (int i = 0; i < adaptors.Count; i++)
                {
                    if (adaptors[i] <= joltage + 3)
                    {
                        int difference = adaptors[i] - joltage;

                        if (difference == 1)
                        {
                            diff1++;
                        }
                        else if (difference == 3)
                        {
                            diff3++;
                        }

                        joltage = adaptors[i];

                        adaptors.RemoveAt(i);

                        break;
                    }
                }
            }

            return diff1 * diff3;
        }

        public static long Distinct(string[] input)
        {
            List<int> adapters = input.ToIntList();

            adapters.Sort();
            adapters.Insert(0, 0);
            adapters.Add(adapters.Max() + 3);

            int before;
            int after;
            int[] diff = new int[3] { 0, 0, 0 };

            List<int> unskippables = new() { 0 };

            for (int i = 1; i < adapters.Count - 1; i++)
            {
                before = adapters[i] - adapters[i - 1];
                ++diff[before - 1];
                after = adapters[i + 1] - adapters[i];

                if (before == 3 || after == 3)
                {
                    unskippables.Add(i);
                }
                else if (before == 2 && after == 2)
                {
                    unskippables.Add(i);
                }
            }

            ++diff[2];
            unskippables.Add(adapters.Count - 1);

            long result = 1;

            for (int i = 0; i < unskippables.Count - 1; i++)
            {
                result *= Valid[unskippables[i + 1] - unskippables[i] + 1];
            }

            return result;
        }
    }
}
