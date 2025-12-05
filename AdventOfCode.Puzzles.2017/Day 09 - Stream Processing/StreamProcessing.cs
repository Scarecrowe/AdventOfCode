namespace AdventOfCode.Puzzles._2017.Day_09___Stream_Processing
{
    using AdventOfCode.Core.Extensions;

    public class StreamProcessing
    {
        public StreamProcessing(string input)
        {
            int start;
            while ((start = input.IndexOf('!')) > -1)
            {
                input = input.Remove(start, 2);
            }

            while ((start = input.IndexOf('<')) > -1)
            {
                int end = input.IndexOf('>') + 1;
                this.NonCanceled += end - start - 2;
                input = input.Remove(start, end - start);
            }

            input = input.Replace(",");

            while ((start = input.IndexOf('}')) > -1)
            {
                this.TotalScore += start;
                input = input.Remove(start - 1, 2);
            }
        }

        public int TotalScore { get; }

        public int NonCanceled { get; }
    }
}
