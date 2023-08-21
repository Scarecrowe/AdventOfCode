namespace AdventOfCode.Puzzles._2019.Day_20___Donut_Maze
{
    using AdventOfCode.Core;

    public class Portal
    {
        public Portal(Vector<int> point, string name, bool inner)
        {
            this.Point = point;
            this.Name = name;
            this.Inner = inner;
        }

        public Portal(Portal portal)
        {
            this.Point = portal.Point.Clone();
            this.Name = portal.Name;
            this.Inner = portal.Inner;
        }

        public Vector<int> Point { get; }

        public string Name { get; }

        public bool Travelled { get; set; }

        public bool Inner { get; }

        public new string ToString() => $"({this.Point.X},{this.Point.Y}), {this.Name}, {(this.Inner ? "Inner" : "Outer")}";
    }
}
