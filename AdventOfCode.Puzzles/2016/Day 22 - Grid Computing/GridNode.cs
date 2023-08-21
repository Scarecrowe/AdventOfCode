namespace AdventOfCode.Puzzles._2016.Day_22___Grid_Computing
{
    using AdventOfCode.Core;

    public class GridNode
    {
        public GridNode(Vector<int> point, int size, int used, int avail, int use)
        {
            this.Point = point;
            this.Size = size;
            this.Used = used;
            this.Avail = avail;
            this.Use = use;
        }

        public Vector<int> Point { get; }

        public int Size { get; }

        public int Used { get; }

        public int Avail { get; }

        public int Use { get; }
    }
}
