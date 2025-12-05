namespace AdventOfCode.Puzzles._2023.Day_03___Gear_Ratios
{
    using AdventOfCode.Core;

    public class Part
    {
        public Part(Vector<int> point, char value)
        {
            this.Point = point;
            this.Value = value;
        }

        public Vector<int> Point { get; }

        public char Value { get; }
    }
}
