namespace AdventOfCode.Animation
{
    using AdventOfCode.Core;

    public class Edges
    {
        public Edges(
            Vector<long> topLeft,
            Vector<long> topRight,
            Vector<long> bottomLeft,
            Vector<long> bottomRight)
        {
            this.TopLeft = topLeft;
            this.TopRight = topRight;
            this.BottomLeft = bottomLeft;
            this.BottomRight = bottomRight;
        }

        public Vector<long> TopLeft { get; }

        public Vector<long> TopRight { get; }

        public Vector<long> BottomLeft { get; }

        public Vector<long> BottomRight { get; }
    }
}
