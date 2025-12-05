namespace AdventOfCode.Puzzles._2023.Day_06___Wait_For_It
{
    using AdventOfCode.Core.Extensions;

    public class WaitForIt
    {
        public WaitForIt(string[] input, bool removeWhiteSpace = false)
        {
            this.Races = this.Parse(input, removeWhiteSpace);
        }

        private List<Race> Races { get; }

        public long Race()
        {
            List<long> result = new();

            foreach(Race race in this.Races)
            {
                long combinations = 0;

                for(int i = 0; i <= race.Time; i += 1)
                {
                    if (i * (race.Time - i) > race.Distance)
                    {
                        combinations++;
                    }
                }

                result.Add(combinations);
            }

            return result.Product();
        }

        private List<Race> Parse(string[] input, bool removeWhiteSpace)
        {
            List<Race> result = new();
            List<long> time = this.StripNumbers(input[0], removeWhiteSpace);
            List<long> distance = this.StripNumbers(input[1], removeWhiteSpace);

            for(int i = 0; i < time.Count; i++)
            {
                result.Add(new(time[i], distance[i]));
            }

            return result;
        }

        private List<long> StripNumbers(string line, bool removeWhiteSpace)
        {
            List<long> result = new();
            string number = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                if(removeWhiteSpace
                    && line[i] == ' ')
                {
                    continue;
                }

                if (char.IsDigit(line[i]))
                {
                    number += line[i];
                    continue;
                }

                if (!string.IsNullOrEmpty(number))
                {
                    result.Add(long.Parse(number));
                    number = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(number))
            {
                result.Add(long.Parse(number));
            }

            return result;
        }
    }
}
