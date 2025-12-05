namespace AdventOfCode.Puzzles._2015.Day_06___Probably_a_Fire_Hazard
{
    using AdventOfCode.Core;

    public class LightGrid : Vector<int>
    {
        public LightGrid(Vector<int> point, int width, int height)
            : base(point)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }

        public int Height { get; }
    }
}
