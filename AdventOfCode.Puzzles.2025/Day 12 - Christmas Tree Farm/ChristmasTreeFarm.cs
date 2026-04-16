namespace AdventOfCode.Puzzles._2025.Day_12___Christmas_Tree_Farm
{
    public sealed class ChristmasTreeFarm
    {
        private readonly string[] input;

        public ChristmasTreeFarm(string[] input)
        {
            this.input = input;
        }

        public long RegionCount()
        {
            int[] presentAreas = ParsePresentAreas(input);

            long answer = 0;

            foreach (string rawLine in input)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                string line = rawLine.Trim();

                if (line.EndsWith(":", StringComparison.Ordinal))
                {
                    continue;
                }

                if (IsShapeRow(line))
                {
                    continue;
                }
                    
                int colonIndex = line.IndexOf(':');

                if (colonIndex < 0)
                {
                    continue;
                }

                ReadOnlySpan<char> dimsSpan = line.AsSpan(0, colonIndex).Trim();
                ReadOnlySpan<char> countsSpan = line.AsSpan(colonIndex + 1).Trim();

                int xIndex = dimsSpan.IndexOf('x');

                if (xIndex < 0)
                {
                    continue;
                }

                int width = ParseInt(dimsSpan.Slice(0, xIndex));
                int height = ParseInt(dimsSpan.Slice(xIndex + 1));

                long boardArea = (long)width * height;
                long requiredArea = 0;

                int presentIndex = 0;
                int current = 0;
                bool inNumber = false;

                for (int i = 0; i < countsSpan.Length; i++)
                {
                    char c = countsSpan[i];

                    if (c >= '0' && c <= '9')
                    {
                        current = (current * 10) + (c - '0');
                        inNumber = true;
                    }
                    else if (inNumber)
                    {
                        if (presentIndex < presentAreas.Length)
                        {
                            requiredArea += (long)current * presentAreas[presentIndex];
                        }

                        presentIndex++;
                        current = 0;
                        inNumber = false;
                    }
                }

                if (inNumber && presentIndex < presentAreas.Length)
                {
                    requiredArea += (long)current * presentAreas[presentIndex];
                }

                if (requiredArea <= boardArea)
                {
                    answer++;
                }
            }

            return answer;
        }

        private static int[] ParsePresentAreas(string[] input)
        {
            List<int> areas = new(6);

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i].Trim();

                if (line.Length == 0)
                {
                    continue;
                }

                if (!line.EndsWith(":", StringComparison.Ordinal))
                {
                    continue;
                }

                bool numericId = true;

                for (int j = 0; j < line.Length - 1; j++)
                {
                    if (line[j] < '0' || line[j] > '9')
                    {
                        numericId = false;
                        break;
                    }
                }

                if (!numericId)
                {
                    continue;
                }

                int area = 0;

                for (int r = i + 1; r < input.Length; r++)
                {
                    string shapeLine = input[r].Trim();

                    if (shapeLine.Length == 0)
                    {
                        break;
                    }

                    if (!IsShapeRow(shapeLine))
                    {
                        break;
                    }

                    for (int c = 0; c < shapeLine.Length; c++)
                    {
                        if (shapeLine[c] == '#')
                        {
                            area++;
                        }
                    }
                }

                areas.Add(area);
            }

            return areas.ToArray();
        }

        private static bool IsShapeRow(string line)
        {
            if (line.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c != '#' && c != '.')
                {
                    return false;
                }                   
            }

            return true;
        }

        private static int ParseInt(ReadOnlySpan<char> span)
        {
            int value = 0;

            for (int i = 0; i < span.Length; i++)
            {
                char c = span[i];

                if (c >= '0' && c <= '9')
                {
                    value = (value * 10) + (c - '0');
                }    
            }

            return value;
        }
    }
}