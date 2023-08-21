namespace AdventOfCode.Puzzles._2021.Day_17___Trick_Shot
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TrickShot
    {
        public TrickShot(string[] input)
        {
            this.Min = new(0, 0);
            this.Max = new(0, 0);
            this.Parse(input);
        }

        public Vector<int> Min { get; private set; }

        public Vector<int> Max { get; private set; }

        public (Vector<int> Point, long Highest) Fire(Vector<int> velocity)
        {
            Vector<int> current = new(0, 0);
            Vector<int> step = new(0, velocity.Y);
            long highest = 0;

            while (current.Y < this.Max.Y * -1)
            {
                current.X += velocity.X - step.X;
                current.Y -= step.Y;
                step.Y--;

                if (current.Y < highest)
                {
                    highest = current.Y;
                }

                if ((current.Y >= this.Min.Y * -1 && current.Y <= this.Max.Y * -1) && (current.X >= this.Min.X && current.X <= this.Max.X))
                {
                    return (velocity, highest);
                }

                if (current.Y > this.Max.Y * -1 || current.X > this.Max.X)
                {
                    return (new(0, 0), 0);
                }

                if (step.X < velocity.X)
                {
                    step.X++;
                }
            }

            return (new(0, 0), 0);
        }

        public long Simulate(bool highest)
        {
            List<(Vector<int> Point, long Highest)> hits = new();

            for (int x = 0; x < 200; x++)
            {
                for (int y = -150; y < 150; y++)
                {
                    (Vector<int> Point, long Highest) location = this.Fire(new(x, y));

                    if (location.Point != new Vector<int>(0, 0))
                    {
                        hits.Add(location);
                    }
                }
            }

            return highest ? hits.Min(x => x.Highest) * -1 : hits.Count();
        }

        private void Parse(string[] input)
        {
            string[] tokens = input[0].Replace("target area: ").Split(", ");
            int[] tokensX = tokens[0].Replace("x=").Split("..").ToInt();
            int[] tokensY = tokens[1].Replace("y=").Split("..").ToInt();

            this.Min = new(tokensX.Min(), tokensY.Max());
            this.Max = new(tokensX.Max(), tokensY.Min());
        }
    }
}
