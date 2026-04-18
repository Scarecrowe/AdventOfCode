namespace AdventOfCode.Puzzles._2023.Day_12___Hot_Springs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AdventOfCode.Core.Extensions;

    public class HotSprings
    {
        private readonly string[] input;

        public HotSprings(string[] input)
        {
            this.input = input;
        }

        public long Arrangements()
        {
            long result = 0;

            foreach (string line in this.input)
            {
                (string pattern, int[] groups) = this.Parse(line, false);
                result += this.CountArrangements(pattern, groups);
            }

            return result;
        }

        public long UnfoldedArrangements()
        {
            long result = 0;

            foreach (string line in this.input)
            {
                (string pattern, int[] groups) = this.Parse(line, true);
                result += this.CountArrangements(pattern, groups);
            }

            return result;
        }

        private (string Pattern, int[] Groups) Parse(string line, bool unfold)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string pattern = tokens[0];
            int[] groups = tokens[1].Split(',').ToInt();

            if (!unfold)
            {
                return (pattern, groups);
            }

            string expandedPattern = string.Join("?", Enumerable.Repeat(pattern, 5));
            int[] expandedGroups = Enumerable.Range(0, 5)
                .SelectMany(_ => groups)
                .ToArray();

            return (expandedPattern, expandedGroups);
        }

        private long CountArrangements(string pattern, int[] groups)
        {
            Dictionary<(int Index, int GroupIndex, int RunLength), long> cache = new();

            return this.Count(pattern, groups, 0, 0, 0, cache);
        }

        private long Count(
            string pattern,
            int[] groups,
            int index,
            int groupIndex,
            int runLength,
            Dictionary<(int Index, int GroupIndex, int RunLength), long> cache)
        {
            (int Index, int GroupIndex, int RunLength) key = (index, groupIndex, runLength);

            if (cache.TryGetValue(key, out long cached))
            {
                return cached;
            }

            if (index == pattern.Length)
            {
                if (runLength > 0)
                {
                    if (groupIndex >= groups.Length || runLength != groups[groupIndex])
                    {
                        return cache[key] = 0;
                    }

                    groupIndex++;
                }

                return cache[key] = groupIndex == groups.Length ? 1 : 0;
            }

            long result = 0;
            char current = pattern[index];

            if (current == '.' || current == '?')
            {
                if (runLength == 0)
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex, 0, cache);
                }
                else if (groupIndex < groups.Length && runLength == groups[groupIndex])
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex + 1, 0, cache);
                }
            }

            if (current == '#' || current == '?')
            {
                if (groupIndex < groups.Length && runLength < groups[groupIndex])
                {
                    result += this.Count(pattern, groups, index + 1, groupIndex, runLength + 1, cache);
                }
            }

            cache[key] = result;
            return result;
        }
    }
}