namespace AdventOfCode.Puzzles._2017.Day_03___Spiral_Memory
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SpiralMemory
    {
        public SpiralMemory(int size)
        {
            this.Point = new(-1, 0);
            this.Min = new(0, 0);
            this.Max = new(0, 0);
            this.Size = size;
            this.Memory = new();
            this.Direction = Cardinal.East;
        }

        public VectorDictionary<int, int> Memory { get; private set; }

        public int Size { get; }

        private Vector<int> Point { get; set; }

        private Cardinal Direction { get; set; }

        private Vector<int> Min { get; }

        private Vector<int> Max { get; }

        public int FillMemory()
        {
            while (true)
            {
                this.Move();

                int sum = this.Memory.AdjacentInterCardinal(this.Point).Sum(x => x.Value);

                if (sum > this.Size)
                {
                    return sum;
                }

                this.Memory.Add(this.Point, sum == 0 ? 1 : sum);
            }
        }

        public long Distance()
        {
            Enumerable.Range(1, this.Size).ForEach(i => this.Move());
            return new Vector<int>(0, 0).Distance(this.Point);
        }

        private void Move()
        {
            switch (this.Direction)
            {
                case Cardinal.East:
                    this.Max.X = this.MoveInDirection(Cardinal.North, this.Max.X, (point, edge) => point.X > edge, (edge) => edge + 1);
                    return;
                case Cardinal.South:
                    this.Max.Y = this.MoveInDirection(Cardinal.East, this.Max.Y, (point, edge) => point.Y > edge, (edge) => edge + 1);
                    return;
                case Cardinal.West:
                    this.Min.X = this.MoveInDirection(Cardinal.South, this.Min.X, (point, edge) => point.X < edge, (edge) => edge - 1);
                    return;
                case Cardinal.North:
                    this.Min.Y = this.MoveInDirection(Cardinal.West, this.Min.Y, (point, edge) => point.Y < edge, (edge) => edge - 1);
                    return;
            }

            throw new InvalidOperationException();
        }

        private int MoveInDirection(
            Cardinal nextDirection,
            int edge,
            Func<Vector<int>, int, bool> equality,
            Func<int, int> incrementor)
        {
            this.Point += CardinalHelper.CardinalTransform<int>()[this.Direction];

            if (equality(this.Point, edge))
            {
                this.Direction = nextDirection;
                return incrementor(edge);
            }

            return edge;
        }
    }
}
