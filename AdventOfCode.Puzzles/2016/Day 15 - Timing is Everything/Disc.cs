namespace AdventOfCode.Puzzles._2016.Day_15___Timing_is_Everything
{
    using AdventOfCode.Core.Extensions;

    public class Disc
    {
        public Disc(string line)
        {
            int[] tokens = line.Replace("Disc #")
                .Replace("has ")
                .Replace("positions; at time=0, it is at position ")
                .Replace(".")
                .SplitSpace()
                .ToInt();

            this.Positions = tokens[1];
            this.CurrentPosition = tokens[2];
        }

        public Disc(int positions, int currentPosition)
        {
            this.Positions = positions;
            this.CurrentPosition = currentPosition;
        }

        public int Positions { get; set; }

        public int CurrentPosition { get; private set; }

        public bool HasBall { get; set; }

        public void SetPosition()
        {
            this.CurrentPosition++;

            if (this.CurrentPosition > this.Positions - 1)
            {
                this.CurrentPosition = 0;
            }
        }
    }
}
