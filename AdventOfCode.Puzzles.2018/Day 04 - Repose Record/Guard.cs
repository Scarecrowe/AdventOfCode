namespace AdventOfCode.Puzzles._2018.Day_04___Repose_Record
{
    public class Guard
    {
        public Guard(int number)
        {
            this.MinutesAsleep = new();
            this.Number = number;
        }

        public int Number { get; }

        public Dictionary<int, int> MinutesAsleep { get; }

        public void AddMinuteAsleep(int minute)
        {
            if (!this.MinutesAsleep.ContainsKey(minute))
            {
                this.MinutesAsleep.Add(minute, 0);
            }

            this.MinutesAsleep[minute]++;
        }
    }
}
