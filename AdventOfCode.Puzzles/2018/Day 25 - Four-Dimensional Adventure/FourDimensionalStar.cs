namespace AdventOfCode.Puzzles._2018.Day_25___Four_Dimensional_Adventure
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class FourDimensionalStar : Vector<int>
    {
        public FourDimensionalStar(string line)
            : base(line.Split(",").ToInt())
        {
            this.Cluster = new();
        }

        public bool IsGrouped { get; private set; }

        public List<FourDimensionalStar> Cluster { get; }

        public void Grouped() => this.IsGrouped = true;
    }
}
