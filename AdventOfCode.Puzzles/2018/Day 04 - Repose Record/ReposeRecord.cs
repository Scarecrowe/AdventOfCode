namespace AdventOfCode.Puzzles._2018.Day_04___Repose_Record
{
    public class ReposeRecord
    {
        public ReposeRecord(string[] input)
        {
            this.Logs = Parse(input);
            this.Guards = new();
            this.Logs = this.Logs.OrderBy(x => x.TimeStamp).ToList();
        }

        public List<Log> Logs { get; }

        public Dictionary<int, Guard> Guards { get; }

        public int Guard { get; private set; }

        public int Asleep { get; private set; }

        public ReposeRecord ProcessLog()
        {
            this.Guard = 0;
            this.Asleep = 0;

            foreach (Log log in this.Logs)
            {
                switch (log.Action)
                {
                    case ActionType.BeginShift:
                        this.BeginShift(log);

                        break;
                    case ActionType.FallAsleep:
                        this.FallAsleep(log);
                        break;
                    case ActionType.WakeUp:
                        this.WakeUp(log);
                        break;
                }
            }

            return this;
        }

        public int StrategyOne()
        {
            int guard = this.Guards.Select(x => (x.Key, x.Value.MinutesAsleep.Sum(y => y.Value))).Aggregate((a, b) => a.Item2 > b.Item2 ? a : b).Key;

            return guard * this.Guards[guard].MinutesAsleep.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
        }

        public int StrategyTwo()
        {
            int guard = this.Guards.Select(x => (x.Key, !x.Value.MinutesAsleep.Any() ? 0 : x.Value.MinutesAsleep.Max(y => y.Value))).Aggregate((a, b) => a.Item2 > b.Item2 ? a : b).Key;

            return guard * this.Guards[guard].MinutesAsleep.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
        }

        private static List<Log> Parse(string[] input) => input.Select(x => new Log(x)).ToList();

        private void BeginShift(Log log)
        {
            this.Guard = log.Guard;

            if (!this.Guards.ContainsKey(this.Guard))
            {
                this.Guards.Add(this.Guard, new(this.Guard));
            }

            if (this.Asleep > 0)
            {
                for (int i = this.Asleep; i < log.TimeStamp.Minute; i++)
                {
                    this.Guards[this.Guard].AddMinuteAsleep(i);
                }
            }

            this.Asleep = 0;
        }

        private void FallAsleep(Log log) => this.Asleep = log.TimeStamp.Minute;

        private void WakeUp(Log log)
        {
            for (int i = this.Asleep; i < log.TimeStamp.Minute; i++)
            {
                this.Guards[this.Guard].AddMinuteAsleep(i);
            }

            this.Asleep = 0;
        }
    }
}
