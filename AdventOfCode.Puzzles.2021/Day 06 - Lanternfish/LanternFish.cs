namespace AdventOfCode.Puzzles._2021.Day_06___Lanternfish
{
    public class LanternFish
    {
        public LanternFish(int internalTimer, long total)
        {
            this.InternalTimer = internalTimer;
            this.Total = total;
        }

        public int InternalTimer { get; set; }

        public long Total { get; set; }
    }
}
