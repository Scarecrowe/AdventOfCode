namespace AdventOfCode.Puzzles._2016.Day_15___Timing_is_Everything
{
    public class TimingIsEverything
    {
        public TimingIsEverything(string[] input, bool extraDisc = false)
        {
            this.Discs = new();

            foreach (string line in input)
            {
                this.Discs.Add(new(line));
            }

            if (extraDisc)
            {
                this.Discs.Add(new(11, 0));
            }
        }

        public List<Disc> Discs { get; private set; }

        public int SimulateBall()
        {
            var time = 0;

            do
            {
                foreach (var disc in this.Discs)
                {
                    disc.SetPosition();
                }

                time++;

                bool ballPresent = false;
                var index = 0;

                while (ballPresent == false && index != this.Discs.Count - 1)
                {
                    if (this.Discs[index].HasBall)
                    {
                        ballPresent = true;
                    }

                    index++;
                }

                if (!ballPresent)
                {
                    if (this.Discs[0].CurrentPosition == 0)
                    {
                        this.Discs[0].HasBall = true;
                    }

                    continue;
                }

                for (int i = 0; i < this.Discs.Count - 1; i++)
                {
                    if (this.Discs[i].HasBall)
                    {
                        this.Discs[i].HasBall = false;

                        if (this.Discs[i + 1].CurrentPosition != 0)
                        {
                            break;
                        }

                        this.Discs[i + 1].HasBall = true;
                        break;
                    }
                }
            }
            while (!this.Discs[this.Discs.Count - 1].HasBall);

            return time - this.Discs.Count;
        }
    }
}
