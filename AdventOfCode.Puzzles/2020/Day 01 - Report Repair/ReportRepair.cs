namespace AdventOfCode.Puzzles._2020.Day_01___Report_Repair
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ReportRepair
    {
        public ReportRepair(string[] input) => this.Input = input.ToInt();

        public int[] Input { get; }

        public int Pair()
            => Enumerator.Range2D(this.Input.Length)
                    .Where(x => this.Input.SumByIndex(x.i, x.j) == 2020)
                    .Select(x => this.Input.ProductByIndex(x.i, x.j))
                    .First();

        public int Tripple()
            => Enumerator.Range3D(this.Input.Length)
                .Where(x => this.Input.SumByIndex(x.i, x.j, x.k) == 2020)
                .Select(x => this.Input.ProductByIndex(x.i, x.j, x.k))
                .First();
    }
}
