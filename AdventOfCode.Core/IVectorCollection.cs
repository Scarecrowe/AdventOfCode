namespace AdventOfCode.Core
{
    public interface IVectorCollection<TSize, TValue>
    {
        TSize Width { get; }

        TSize Height { get; }

        TValue this[long y, long x] { get; set; }

        TValue this[Vector<TSize> point] { get; set; }

        void Add(Vector<TSize> key, TValue value);

        IEnumerable<VectorCell<TSize, TValue>> GetRow(TSize y);

        IEnumerable<VectorCell<TSize, TValue>> GetColumn(TSize x);

        IEnumerable<VectorCell<TSize, TValue>> AdjacentCardinal(Vector<TSize> point);

        IEnumerable<VectorCell<TSize, TValue>> AdjacentInterCardinal(Vector<TSize> point);

        IEnumerable<VectorCell<TSize, TValue>> Letters();

        bool IsVectorInRange(TSize x, TSize y);

        bool IsVectorInRange(Vector<TSize> point);

        bool IsVectorInRange((TSize X, TSize Y) point);

        string Print(Func<TValue, char> comparer);

        IEnumerable<VectorCell<TSize, TValue>> AxisEnumerator();

        IEnumerable<List<VectorCell<TSize, TValue>>> RowEnumerator();

        IEnumerable<List<VectorCell<TSize, TValue>>> ColumnEnumerator();

        IEnumerable<VectorCell<TSize, TValue>> EdgeEnumerator();

        IEnumerable<TValue?> Flatten();

        BreadthFirstSearchResult<TSize> BreadthFirstSearch(Vector<TSize> source, Vector<TSize> destination, List<Vector<TSize>> blocked);

        bool IsEdge(TValue value);

        bool IsEdge(Vector<TSize> point);

        void Resize(TSize width, TSize height);

        void Clear();
    }
}
