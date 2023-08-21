namespace AdventOfCode.Puzzles._2020.Day_14___Docking_Data
{
    using System.Text.RegularExpressions;
    using AdventOfCode.Core.Extensions;

    public class DockingData
    {
        public static long Version1(string[] input)
        {
            long[] memory = new long[100000];
            string mask = string.Empty;
            long maskAnd = 0;
            long maskOr = 0;

            foreach (string instruction in input)
            {
                string[] tokens = instruction.Split("=");

                if (tokens[0].StartsWith("mask"))
                {
                    mask = tokens[1].Trim();

                    maskAnd = Convert.ToInt64(mask.Replace("X", "1"), 2);
                    maskOr = Convert.ToInt64(mask.Replace("X", "0"), 2);
                }
                else
                {
                    int address = tokens[0].Replace("mem[").Replace("]").ToInt();
                    uint value = tokens[1].Trim().ToUInt();

                    long result = value & maskAnd;
                    result |= maskOr;

                    memory[address] = result;
                }
            }

            return memory.Where(x => x > 0).Sum();
        }

        public static long Version2(string[] input)
        {
            string mask = string.Empty;
            Dictionary<long, long> memory = new();

            long numberOfCombinations = 0;

            foreach (string line in input)
            {
                if (line.StartsWith("mask = "))
                {
                    mask = Regex.Match(line, "mask = ([01X]+)").Groups[1].Value;
                    numberOfCombinations = (long)Math.Pow(2, mask.Count(x => x == 'X'));
                }
                else
                {
                    Match match = Regex.Match(line, "mem\\[(\\d+)\\] = (\\d+)");
                    long memoryAddress = match.Groups[1].Value.ToLong();
                    long value = match.Groups[2].Value.ToLong();

                    for (long i = 0; i < numberOfCombinations; i++)
                    {
                        long memoryAddressCopy = memoryAddress;

                        int offset = 0;

                        for (int x = 0; x < mask.Length; x++)
                        {
                            char c = mask[mask.Length - 1 - x];

                            if (c == '0')
                            {
                            }
                            else if (c == '1')
                            {
                                memoryAddressCopy |= 1L << x;
                            }
                            else if (c == 'X')
                            {
                                long status = (i >> offset) & 1;

                                if (status == 0)
                                {
                                    memoryAddressCopy &= ~(1L << x);
                                }
                                else
                                {
                                    memoryAddressCopy |= 1L << x;
                                }

                                offset++;
                            }
                        }

                        if (!memory.ContainsKey(memoryAddressCopy))
                        {
                            memory.Add(memoryAddressCopy, 0);
                        }

                        memory[memoryAddressCopy] = value;
                    }
                }
            }

            return memory.Values.Sum();
        }
    }
}
