namespace AdventOfCode.Puzzles._2023.Day_06___Wait_For_It
{
    public class Race
    {
        public Race(long time, long distance)
        {
            this.Time = time;
            this.Distance = distance;
        }

        public long Time { get; }

        public long Distance { get; }
    }
}
