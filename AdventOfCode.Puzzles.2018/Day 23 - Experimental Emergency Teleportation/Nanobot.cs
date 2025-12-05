namespace AdventOfCode.Puzzles._2018.Day_23___Experimental_Emergency_Teleportation
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Nanobot
    {
        public Nanobot(string line)
        {
            string[] tokens = line.Split(", ");
            this.Point = new Vector<int>(tokens[0].In("0123456789-,").Split(",").ToInt());
            this.Radius = tokens[1].In("0123456789").ToInt();
        }

        public Vector<int> Point { get; }

        public int Radius { get; }

        public int Count { get; set; }

        public bool Contains(Vector<int> point)
            => (point.X >= this.Point.X) && (point.X <= this.Point.X + this.Radius) &&
                    (point.Y >= this.Point.Y) && (point.Y <= this.Point.Y + this.Radius) &&
                    (point.Z >= this.Point.Z) && (point.Z <= this.Point.Z + this.Radius);
    }
}
