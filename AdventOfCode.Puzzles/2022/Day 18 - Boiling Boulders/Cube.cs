namespace AdventOfCode.Puzzles._2022.Day_18___Boiling_Boulders
{
    using AdventOfCode.Core;

    public class Cube
    {
        public Cube(Vector<int> point) => this.Point = point;

        public Vector<int> Point { get; }

        public CubeType Type { get; private set; }

        public void SetType(CubeType type) => this.Type = type;
    }
}
