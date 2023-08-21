namespace AdventOfCode.Core
{
    public class VectorCell<TSize, TValue>
    {
        public VectorCell(Vector<TSize> point, TValue value)
        {
            this.Point = point;
            this.Value = value;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public VectorCell(Vector<TSize> point, Cardinal direction)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.Point = point;
            this.Direction = direction;
        }

        public VectorCell(Vector<TSize> point, TValue value, Cardinal direction)
            : this(point, value)
        {
            this.Direction = direction;
        }

        public Vector<TSize> Point { get; }

        public TValue Value { get; }

        public Cardinal Direction { get; }

        public (TSize X, TSize Y) ToTuple() => (this.Point.X, this.Point.Y);

        public override int GetHashCode() => System.HashCode.Combine(this.Point.X, this.Point.Y, this.Value, this.Direction);
    }
}
