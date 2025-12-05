namespace AdventOfCode.Puzzles._2015.Day_14___Reindeer_Olympics
{
    using AdventOfCode.Core.Extensions;

    public class ReindeerOlympics
    {
        public ReindeerOlympics(string[] input, int seconds)
        {
            this.Reindeers = Parse(input);
            this.Seconds = seconds;
        }

        private Dictionary<string, Reindeer> Reindeers { get; }

        private int Seconds { get; }

        public long RaceByDistance()
        {
            Enumerable.Range(1, this.Seconds).ForEach(second => this.Reindeers.ForEach(x => x.Value.Travel()));

            return this.MaxDistance();
        }

        public long RaceByScore()
        {
            Enumerable.Range(1, this.Seconds).ForEach(second => this.Race());

            return this.MaxScore();
        }

        private static Dictionary<string, Reindeer> Parse(string[] input)
        {
            Dictionary<string, Reindeer> result = new();

            foreach (string line in input)
            {
                string[] tokens = line.SplitSpace();

                Reindeer reindeer = new(tokens[0], tokens[3].ToInt(), tokens[6].ToInt(), tokens[13].ToInt());

                result.Add(reindeer.Name, reindeer);
            }

            return result;
        }

        private long MaxDistance() => this.Reindeers.Values.Select(x => x.TotalDistance).Max();

        private long MaxScore() => this.Reindeers.Values.Select(x => x.Score).Max();

        private void Race()
        {
            this.Reindeers.ForEach(x => x.Value.Travel());
            long max = this.MaxDistance();
            this.Reindeers.Where(x => x.Value.TotalDistance == max).ForEach(x => x.Value.IncrementScore());
        }
    }
}
