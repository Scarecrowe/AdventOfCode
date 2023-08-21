namespace AdventOfCode.Puzzles._2015.Day_14___Reindeer_Olympics
{
    public class Reindeer
    {
        public Reindeer(string name, int distance, int seconds, int rest)
        {
            this.Name = name;
            this.Distance = distance;
            this.Seconds = seconds;
            this.Rest = rest;
            this.TotalSeconds = 0;
            this.TotalRest = 0;
            this.TotalDistance = 0;
            this.Score = 0;
        }

        public string Name { get; }

        public long TotalDistance { get; private set; }

        public int TotalSeconds { get; private set; }

        public int TotalRest { get; private set; }

        public int Distance { get; private set; }

        public int Seconds { get; private set; }

        public int Rest { get; private set; }

        public int Score { get; private set; }

        public void IncrementScore() => this.Score++;

        public void Travel()
        {
            if (this.TotalRest == this.Rest)
            {
                this.TotalSeconds = 0;
                this.TotalRest = 0;
            }

            if (this.TotalSeconds < this.Seconds)
            {
                this.TotalSeconds++;
                this.TotalDistance += this.Distance;

                return;
            }

            if (this.TotalRest < this.Rest)
            {
                this.TotalRest++;

                return;
            }
        }
    }
}
