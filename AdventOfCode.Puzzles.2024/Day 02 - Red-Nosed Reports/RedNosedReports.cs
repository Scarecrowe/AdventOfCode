namespace AdventOfCode.Puzzles._2024.Day_02___Red_Nosed_Reports
{
    public class RedNosedReports
    {
        public List<List<int>> Levels { get; private set; }

        public RedNosedReports(string[] input)
        {
            this.Levels = input.Select(x => x.Split(" ").Select(x => int.Parse(x)).ToList()).ToList();
        }

        public int Safe(bool tolerate = false)
        {
            int result = 0;

            foreach (List<int> level in this.Levels)
            {
                if (IsSafe(level))
                {
                    result++;
                    continue;
                }

                if (tolerate)
                {
                    for (int i = 0; i < level.Count; i++)
                    {
                        List<int> clone = [.. level];

                        clone.RemoveAt(i);

                        if (IsSafe(clone))
                        {
                            result++;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsSafe(List<int> values)
        {
            bool decreasing = (values[0] - values[1]) > 0;

            for (int i = 0; i < values.Count - 1; i++)
            {
                int value = values[i] - values[i + 1];

                if (Math.Abs(value) > 3
                    || decreasing != (value > 0)
                    || values[i] == values[i + 1])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
