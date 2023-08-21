namespace AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus
{
    using AdventOfCode.Core;

    public class DumboOctopus
    {
        public DumboOctopus(Vector<int> point, int energyLevel, Cavern cavern)
        {
            this.EnegryLevel = energyLevel;
            this.Cavern = cavern;
            this.Point = point;
        }

        public Vector<int> Point { get; }

        public int EnegryLevel { get; private set; }

        public bool Flashed { get; private set; }

        public Cavern Cavern { get; }

        public void Increment()
        {
            if (this.Flashed)
            {
                return;
            }

            this.EnegryLevel++;

            if (!this.Flashed && this.EnegryLevel > 9)
            {
                this.Flashed = true;
                this.EnegryLevel = 0;
                this.Cavern.Map.IncrementFlashes();

                foreach (VectorCell<int, DumboOctopus> octopus in this.Cavern.Map.AdjacentInterCardinal(this.Point))
                {
                    octopus.Value.Increment();
                }
            }
        }

        public void Reset() => this.Flashed = false;
    }
}
