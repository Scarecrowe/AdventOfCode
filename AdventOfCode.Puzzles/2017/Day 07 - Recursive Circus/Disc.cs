namespace AdventOfCode.Puzzles._2017.Day_07___Recursive_Circus
{
    public class Disc
    {
        public Disc(string name, int weight)
        {
            this.Name = name;
            this.Weight = weight;
            this.Children = new();
        }

        public string Name { get; }

        public int Weight { get; }

        public int TotalWeight { get; private set; }

        public List<string> Children { get; }

        public void CalculateTotalWeight(ref Dictionary<string, Disc> discs) => this.TotalWeight = this.Weigh(ref discs);

        private int Weigh(ref Dictionary<string, Disc> discs, Disc? disc = null)
        {
            int weight = 0;

            disc ??= this;

            weight += disc.Weight;

            foreach (string child in disc.Children)
            {
                weight += this.Weigh(ref discs, discs[child]);
            }

            return weight;
        }
    }
}
