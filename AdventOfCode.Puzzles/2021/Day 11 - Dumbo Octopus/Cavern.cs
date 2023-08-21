namespace AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus
{
    using AdventOfCode.Core;

    public class Cavern
    {
        public Cavern(string[] input) => this.Map = new(this, input);

        public DumboOctopusMap Map { get; private set; }

        public Cavern RunFor(int iterations)
        {
            for (int i = 1; i <= iterations; i++)
            {
                this.RunStep();
            }

            return this;
        }

        public int RunUntil()
        {
            int steps = 1;

            while (true)
            {
                if (this.RunStep())
                {
                    return steps;
                }

                if (steps > 300)
                {
                    break;
                }

                steps++;
            }

            return -1;
        }

        private bool RunStep()
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width + 1; x++)
                {
                    this.Map[new(x, y)].Increment();
                }
            }

            bool all = true;

            foreach (KeyValuePair<Vector<int>, DumboOctopus> octopus in this.Map)
            {
                if (all && !octopus.Value.Flashed)
                {
                    all = false;
                }

                octopus.Value.Reset();
            }

            return all;
        }
    }
}
