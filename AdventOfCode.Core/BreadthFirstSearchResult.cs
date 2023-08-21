namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Contracts;

    public class BreadthFirstSearchResult<TSize>
    {
        public BreadthFirstSearchResult(List<Vector<TSize>> path)
        {
            path.Should().Not().BeNull(paramName: nameof(path));

            this.Path = path;
        }

        public List<Vector<TSize>> Path { get; }

        public long Distance => this.Path.Count;
    }
}
