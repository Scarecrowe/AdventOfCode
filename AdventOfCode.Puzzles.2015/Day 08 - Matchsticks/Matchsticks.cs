namespace AdventOfCode.Puzzles._2015.Day_08___Matchsticks
{
    using System.Text;
    using System.Text.RegularExpressions;

    public class Matchsticks
    {
        public static int CountCharacters(string input, MatchstickMode mode)
        {
            int characterCount = 0;
            int memoryCount = 0;
            int escapedCount = 0;
            char last = '\0';
            int index = 0;
            StringBuilder normal = new();
            StringBuilder escaped = new();

            do
            {
                char current = input[index];
                index++;

                if (current == '\n')
                {
                    continue;
                }

                if (current != '\r')
                {
                    switch (current)
                    {
                        case '\"':
                            escaped.Append("\\\"");
                            break;
                        case '\\':
                            escaped.Append("\\\\");
                            break;
                        default:
                            escaped.Append(current);
                            break;
                    }

                    characterCount++;

                    if (last == '\\')
                    {
                        normal.Append($"{last}{current}");
                        last = '\0';
                        continue;
                    }
                    else
                    {
                        if (current == '\"' || current == '\\')
                        {
                            last = current;
                            continue;
                        }

                        normal.Append(current);
                    }
                }
                else
                {
                    memoryCount += Regex.Unescape(normal.ToString()).Length;
                    escapedCount += escaped.Length + 2;

                    normal.Clear();
                    escaped.Clear();
                }

                last = current;
            }
            while (index < input.Length);

            if (normal.Length > 0)
            {
                memoryCount += Regex.Unescape(normal.ToString()).Length;
                escapedCount += escaped.Length + 2;
            }

            return mode == MatchstickMode.Normal
                ? characterCount - memoryCount
                : escapedCount - characterCount;
        }
    }
}
