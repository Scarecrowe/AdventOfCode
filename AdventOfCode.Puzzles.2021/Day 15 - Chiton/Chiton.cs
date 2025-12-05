namespace AdventOfCode.Puzzles._2021.Day_15___Chiton
{
    using AdventOfCode.Core;

    public class Chiton
    {
        public Chiton(Vector<int> point, int risk)
        {
            this.Point = point;
            this.Risk = risk;
            this.TotalRisk = int.MaxValue;
        }

        public Vector<int> Point { get; }

        public int Risk { get; }

        public bool Visited { get; set; }

        public int TotalRisk { get; set; }
    }
}
