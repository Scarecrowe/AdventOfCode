namespace AdventOfCode.Puzzles._2015.Day_03___Perfectly_Spherical_Houses_in_a_Vacuum
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class PerfectlySphericalHousesInAVacuum
    {
        public PerfectlySphericalHousesInAVacuum(string[] input, bool robotSanta = false)
        {
            this.Directions = input[0];
            this.Houses = new() { { new(0, 0), robotSanta ? 2 : 0 } };
            this.Santas = new() { new() };

            if (robotSanta)
            {
                this.Santas.Add(new());
            }
        }

        public string Directions { get; private set; }

        public VectorDictionary<int, int> Houses { get; private set; }

        public List<Vector<int>> Santas { get; private set; }

        public PerfectlySphericalHousesInAVacuum Deliver()
        {
            int index = 0;

            foreach (char direction in this.Directions)
            {
                this.Santas[index].SymbolTransform(direction);
                this.Houses.AddOrIncrement(this.Santas[index]);
                index = index.IncrementWrap(this.Santas.Count);
            }

            return this;
        }
    }
}
