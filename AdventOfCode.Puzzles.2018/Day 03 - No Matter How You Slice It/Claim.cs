namespace AdventOfCode.Puzzles._2018.Day_03___No_Matter_How_You_Slice_It
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Claim
    {
        public Claim(string line)
        {
            string[] tokens = line.Split(" @ ");

            this.Id = tokens[0].Replace("#").ToInt();

            tokens = tokens[1].Split(": ");

            int[] position = tokens[0].Split(",").ToInt();

            this.Point = new(position);

            int[] size = tokens[1].Split("x").ToInt();

            this.Width = size[0];
            this.Height = size[1];
        }

        public int Id { get; }

        public Vector<int> Point { get; }

        public int Width { get; }

        public int Height { get; }
    }
}
