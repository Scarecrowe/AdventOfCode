namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Contracts;

    public class BreadthFirstSearchItem<TSize>
    {
        public BreadthFirstSearchItem()
        {
            this.Point = new(0, 0);
            this.Distance = 0;
            this.Path = new();
        }

        public BreadthFirstSearchItem(Vector<TSize> point, long distance, List<Vector<TSize>> path)
        {
            point.Should().Not().BeNull(paramName: nameof(point));
            distance.Should().Not().BeLessThanZero(paramName: nameof(distance));
            path.Should().Not().BeNull(paramName: nameof(path));

            this.Point = point;
            this.Distance = distance;
            this.Path = new(path);

            if (!this.Path.Contains(this.Point))
            {
                this.Path.Add(this.Point);
            }
        }

        public BreadthFirstSearchItem(Vector<TSize> point, long distance)
        {
            point.Should().Not().BeNull(paramName: nameof(point));
            distance.Should().Not().BeLessThanZero(paramName: nameof(distance));

            this.Point = point;
            this.Distance = distance;
            this.Path = new() { point.Clone() };
        }

        public Vector<TSize> Point { get; }

        public long Distance { get; }

        public List<Vector<TSize>> Path { get; }

        public void AddPath(Vector<TSize> point)
        {
            point.Should().Not().BeNull(paramName: nameof(point));

            this.Path.Add(point);
        }
    }
}
