namespace AdventOfCode.Puzzles._2018.Day_20___A_Regular_Map
{
    using AdventOfCode.Core;

    public class Node
    {
        public Node(Vector<int> point, int distance)
        {
            this.Point = point;
            this.Distance = distance;
        }

        public Vector<int> Point { get; }

        public int Distance { get; set; }
    }
}
