namespace AdventOfCode.Core
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class VectorArray<TSize, TValue> : IVectorCollection<TSize, TValue>
    {
        public VectorArray()
        {
            this.Data = new TValue[0, 0];
            this.Width = 0.ToGeneric<TSize>();
            this.Height = 0.ToGeneric<TSize>();
        }

        public VectorArray(TSize width, TSize height)
        {
            this.Width = width;
            this.Height = height;
            this.Data = new TValue[this.Height.ToInt(), this.Width.ToInt()];
        }

        public VectorArray(TSize width, TSize height, TValue defaultValue)
            : this(width, height)
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                this[cell.Point] = defaultValue;
            }
        }

        public VectorArray(string[] input, Func<char, TValue> comparer)
            : this() => this.Create(input, comparer);

        public VectorArray(string[] input, Func<char, int, int, TValue> comparer)
            : this() => this.Create(input, comparer);

        public VectorArray(string[] input, string key)
            : this() => this.Create(input, (x) => (TValue?)Enum.GetValues(typeof(TValue)).GetValue(key.IndexOf(x)));

        public VectorArray(string input, string key)
            : this() => this.Create(new string[] { input }, (x) => (TValue?)Enum.GetValues(typeof(TValue)).GetValue(key.IndexOf(x)));

        public VectorArray(VectorArray<TSize, TValue> array)
            : this()
        {
            if (array.Data != null)
            {
                this.Data = (TValue[,])array.Data.Clone();
            }

            this.Width = array.Width;
            this.Height = array.Height;
        }

        public TSize Width { get; private set; }

        public TSize Height { get; private set; }

        private TValue[,] Data { get; set; }

        public TValue this[long y, long x]
        {
            get
            {
                return this.Data[y, x];
            }

            set
            {
                this.Data[y, x] = value;
            }
        }

        public TValue this[Vector<TSize> point]
        {
            get
            {
                return this.Data == null
                    ? (TValue)(object)0
                    : this.Data[point.Y.ToLong(), point.X.ToLong()];
            }

            set
            {
                this.Data[point.Y.ToLong(), point.X.ToLong()] = value ?? (TValue)(object)0;
            }
        }

        public void Resize(TSize width, TSize height)
        {
            var data = new TValue[height.ToInt(), width.ToInt()];

            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                if (cell.Point.X.ToInt() >= width.ToInt()
                    || cell.Point.Y.ToInt() >= height.ToInt())
                {
                    continue;
                }

                data[cell.Point.Y.ToInt(), cell.Point.X.ToInt()] = cell.Value;
            }

            this.Data = data;
            this.Width = width;
            this.Height = height;
        }

        public void Clear()
            => this.Data = new TValue[this.Height.ToInt(), this.Width.ToInt()];

        public IEnumerable<VectorCell<TSize, TValue>> AdjacentCardinal(Vector<TSize> point)
            => this.Adjacent(CardinalHelper.CardinalCells<TSize, TValue>(), point);

        public IEnumerable<VectorCell<TSize, TValue>> AdjacentInterCardinal(Vector<TSize> point)
        {
            foreach (VectorCell<TSize, TValue> item in this.Adjacent(CardinalHelper.CardinalCells<TSize, TValue>(), point))
            {
                yield return item;
            }

            foreach (VectorCell<TSize, TValue> item in this.Adjacent(CardinalHelper.InterCardinalCells<TSize, TValue>(), point))
            {
                yield return item;
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> Letters()
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                if (char.IsLetter($"{cell.Value}"[0]))
                {
                    yield return cell;
                }
            }
        }

        public string Print(Func<TValue, char> comparer)
        {
            StringBuilder result = new();
            result.AppendLine().AppendLine();

            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    result.Append(comparer(this[y, x]));
                }

                result.AppendLine();
            }

            result.AppendLine();

            return result.ToString();
        }

        public VectorArray<TSize, TValue> Clone() => new(this);

        public IEnumerable<VectorCell<TSize, TValue>> Values(TValue value)
        {
            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    if (this.Data?[y, x]?.Equals(value) ?? false)
                    {
                        yield return new VectorCell<TSize, TValue>(new(x, y), value);
                    }
                }
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> Values(IEnumerable<TValue> values)
        {
            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    if (values.Contains(this.Data[y, x]))
                    {
                        yield return new VectorCell<TSize, TValue>(new(x, y), this.Data[y, x]);
                    }
                }
            }
        }

        public int Count(TValue value) => this.Values(value).Count();

        public int Count(IEnumerable<TValue> values) => this.Values(values).Count();

        public bool Equals(VectorArray<TSize, TValue> array)
        {
            for (long y = 0; y < this.Height.ToInt(); y++)
            {
                for (long x = 0; x < this.Width.ToInt(); x++)
                {
                    if (!array[y, x]?.Equals(this.Data[y, x]) ?? false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public IEnumerable<VectorCell<TSize, TValue>> AxisEnumerator()
        {
            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    yield return new(new(x, y), this[y, x]);
                }
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> AxisEnumerator(Action column)
        {
            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    yield return new(new(x, y), this[y, x]);
                }

                column();
            }
        }

        public IEnumerable<List<VectorCell<TSize, TValue>>> RowEnumerator()
        {
            for (long y = 0; y < this.Height.ToInt(); y++)
            {
                yield return this.GetRow(y.ToGeneric<TSize>()).ToList();
            }
        }

        public IEnumerable<List<VectorCell<TSize, TValue>>> ColumnEnumerator()
        {
            for (long x = 0; x < this.Width.ToInt(); x++)
            {
                yield return this.GetColumn(x.ToGeneric<TSize>()).ToList();
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> EdgeEnumerator()
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                if (cell.Point.X.ToLong() == 0 || cell.Point.Y.ToLong() == 0 || cell.Point.X.ToLong() == this.Width.ToInt() - 1 || cell.Point.Y.ToLong() == this.Height.ToInt() - 1)
                {
                    yield return cell;
                }
            }
        }

        public TValue GetValue(Vector<TSize> point) => this[point];

        public TValue GetValue(TSize x, TSize y) => this.GetValue(new(x, y));

        public IEnumerable<VectorCell<TSize, TValue>> GetRow(TSize y)
        {
            for (long x = 0; x < this.Width.ToInt(); x++)
            {
                yield return new(new(x.ToGeneric<TSize>(), y), this[y.ToLong(), x]);
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> GetColumn(TSize x)
        {
            for (long y = 0; y < this.Height.ToInt(); y++)
            {
                yield return new(new(x, y.ToGeneric<TSize>()), this[y, x.ToLong()]);
            }
        }

        public VectorCell<TSize, TValue>? FirstOrDefault(TValue value)
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                if (cell?.Value?.Equals(value) ?? false)
                {
                    return cell;
                }
            }

            return default;
        }

        public IEnumerable<TValue?> Flatten()
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                yield return cell.Value;
            }
        }

        public BreadthFirstSearchResult<TSize> BreadthFirstSearch(Vector<TSize> source, Vector<TSize> destination, List<Vector<TSize>> blocked)
        {
            Queue<BreadthFirstSearchItem<TSize>> queue = new();

            VectorArray<TSize, bool> visited = new(this.Width, this.Height);
            visited[source.Y.ToLong(), source.X.ToLong()] = true;

            foreach (Vector<TSize> point in blocked)
            {
                visited[point] = true;
            }

            queue.Enqueue(new BreadthFirstSearchItem<TSize>(source, 0));

            while (queue.Count > 0)
            {
                BreadthFirstSearchItem<TSize> item = queue.Dequeue();

                if (item.Point == destination)
                {
                    return new BreadthFirstSearchResult<TSize>(item.Path);
                }

                Vector<TSize> location = new(item.Point.X.ToInt(), item.Point.Y.ToInt() - 1);

                if (this.IsValidBreadthFirstSearch(location, visited))
                {
                    queue.Enqueue(new(location, item.Distance + 1, item.Path));
                    visited[location.X.ToInt(), location.Y.ToInt()] = true;
                }

                location = new(item.Point.X.ToInt(), item.Point.Y.ToInt() + 1);

                if (this.IsValidBreadthFirstSearch(location, visited))
                {
                    queue.Enqueue(new(location, item.Distance + 1, item.Path));
                    visited[location.X.ToInt(), location.Y.ToInt()] = true;
                }

                location = new(item.Point.X.ToInt() - 1, item.Point.Y.ToInt());

                if (this.IsValidBreadthFirstSearch(location, visited))
                {
                    queue.Enqueue(new(location, item.Distance + 1, item.Path));
                    visited[location.X.ToInt(), location.Y.ToInt()] = true;
                }

                location = new(item.Point.X.ToInt() + 1, item.Point.Y.ToInt());

                if (this.IsValidBreadthFirstSearch(location, visited))
                {
                    queue.Enqueue(new(location, item.Distance + 1, item.Path));
                    visited[location.X.ToInt(), location.Y.ToInt()] = true;
                }
            }

            return new BreadthFirstSearchResult<TSize>(new());
        }

        public string ToString(Func<TValue?, char> comparer)
        {
            StringBuilder result = new();

            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                result.Append(comparer(cell.Value));
            }

            return result.ToString();
        }

        public TValue Sum()
        {
            if (typeof(TValue) == typeof(int))
            {
                return this.SumInt();
            }

            return this.SumLong();
        }

        public void Add(Vector<TSize> key, TValue value)
        {
            if (this.IsVectorInRange(key))
            {
                this[key] = value;
            }
        }

        public bool IsVectorInRange(TSize x, TSize y)
            => x.ToLong() >= 0 && x.ToLong() < this.Data?.GetLength(1) && y.ToLong() >= 0 && y.ToLong() < this.Data?.GetLength(0);

        public bool IsVectorInRange(Vector<TSize> point)
            => point.X.ToLong() >= 0 && point.X.ToLong() < this.Data?.GetLength(1) && point.Y.ToLong() >= 0 && point.Y.ToLong() < this.Data?.GetLength(0);

        public bool IsVectorInRange((TSize X, TSize Y) point)
            => point.X.ToLong() >= 0 && point.X.ToLong() < this.Data?.GetLength(1) && point.Y.ToLong() >= 0 && point.Y.ToLong() < this.Data?.GetLength(0);

        public bool IsEdge(TValue value)
            => this.EdgeEnumerator().Any(x => x.Value?.Equals(value) ?? false);

        public bool IsEdge(Vector<TSize> point)
            => this.EdgeEnumerator().Any(x => x.Point == point);

        private IEnumerable<VectorCell<TSize, TValue>> Adjacent(List<VectorCell<TSize, TValue>> cells, Vector<TSize> point)
        {
            foreach (VectorCell<TSize, TValue> cell in cells)
            {
                Vector<TSize> transformed = point + cell.Point;

                if ((transformed.X.ToLong() >= 0 && transformed.X.ToLong() < this.Width.ToLong()) && (transformed.Y.ToLong() >= 0 && transformed.Y.ToLong() < this.Height.ToLong()))
                {
                    yield return new VectorCell<TSize, TValue>(transformed, this[transformed], cell.Direction);
                }
            }
        }

        private void Create(string[] input, Func<char, TValue?> comparer)
        {
            this.SetDimensions(input);
            this.Data = new TValue[this.Height.ToInt(), this.Width.ToInt()];

            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    this.Data[y, x] = comparer(input[y][x]) ?? (TValue)new object();
                }
            }
        }

        private void Create(string[] input, Func<char, int, int, TValue?> comparer)
        {
            this.SetDimensions(input);
            this.Data = new TValue[this.Height.ToInt(), this.Width.ToInt()];

            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    this.Data[y, x] = comparer(input[y][x], x, y) ?? (TValue)new object();
                }
            }
        }

        private void SetDimensions(string[] input)
        {
            this.Width = input[0].Length.ToGeneric<TSize>();
            this.Height = input.Length.ToGeneric<TSize>();
        }

        private TValue SumInt() => (TValue)(object)this.AxisEnumerator().Sum(x => int.Parse($"{x.Value}"));

        private TValue SumLong() => (TValue)(object)this.AxisEnumerator().Sum(x => long.Parse($"{x.Value}"));

        private bool IsValidBreadthFirstSearch(Vector<TSize> point, VectorArray<TSize, bool> visited)
            => point.X.ToLong() >= 0 && point.Y.ToLong() >= 0 && point.X.ToLong() < this.Width.ToLong() && point.Y.ToLong() < this.Height.ToLong() && visited[point] == false;
    }
}
