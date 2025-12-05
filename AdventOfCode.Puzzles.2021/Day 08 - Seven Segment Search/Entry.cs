namespace AdventOfCode.Puzzles._2021.Day_08___Seven_Segment_Search
{
    using AdventOfCode.Core.Extensions;

    public class Entry
    {
        public Entry(string input)
        {
            this.SignalPatterns = new();
            this.OutputValues = new();

            string[] tokens = input.Split(" | ");
            string[] patterns = tokens[0].SplitSpace();
            string[] values = tokens[1].SplitSpace();

            foreach (string pattern in patterns)
            {
                this.SignalPatterns.Add(pattern);
            }

            foreach (string value in values)
            {
                this.OutputValues.Add(value);
            }
        }

        public List<string> SignalPatterns { get; }

        public List<string> OutputValues { get; }
    }
}
