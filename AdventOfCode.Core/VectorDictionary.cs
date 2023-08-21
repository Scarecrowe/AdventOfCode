namespace AdventOfCode.Core
{
    using System.Text;
    using AdventOfCode.Core.Contracts;
    using AdventOfCode.Core.Extensions;

    public class VectorDictionary<TSize, TValue> : Dictionary<Vector<TSize>, TValue>, IVectorCollection<TSize, TValue>
    {
        public VectorDictionary()
        {
            this.Width = 0.ToGeneric<TSize>();
            this.Height = 0.ToGeneric<TSize>();
        }

        public VectorDictionary(int width, int height)
        {
            this.Width = width.ToGeneric<TSize>();
            this.Height = height.ToGeneric<TSize>();
        }

        public VectorDictionary(TSize width, TSize height)
        {
            this.Width = width;
            this.Height = height;
        }

        public VectorDictionary(TSize width, TSize height, TValue defaultValue)
            : this(width, height)
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                this[cell.Point] = defaultValue;
            }
        }

        public VectorDictionary(VectorDictionary<TSize, TValue> dictionary)
            : this()
        {
            dictionary.Should().Not().BeNull(paramName: nameof(dictionary));

            foreach(KeyValuePair<Vector<TSize>, TValue> pair in dictionary)
            {
                this.Add(pair.Key, pair.Value);
            }

            this.Width = dictionary.Width;
            this.Height = dictionary.Height;
        }

        public VectorDictionary(string[] input, Func<char, TValue?> comparer)
            : this() => this.Create(input, comparer);

        public VectorDictionary(string[] input, Func<char, TSize, TSize, TValue> comparer)
            : this() => this.Create(input, comparer);

        public VectorDictionary(string[] input, string key)
            : this() => this.Create(input, (x) => (TValue?)Enum.GetValues(typeof(TValue)).GetValue(key.IndexOf(x)));

        public VectorDictionary(string input, string key)
            : this() => this.Create(new string[] { input }, (x) => (TValue?)Enum.GetValues(typeof(TValue)).GetValue(key.IndexOf(x)));

        public TSize Width { get; private set; }

        public TSize Height { get; private set; }

        public TValue this[long y, long x]
        {
            get
            {
                return this[new Vector<TSize>(x.ToGeneric<TSize>(), y.ToGeneric<TSize>())];
            }

            set
            {
                if (this.ContainsKey(new Vector<TSize>(x, y)))
                {
                    this[new Vector<TSize>(x, y)] = value;

                    return;
                }

                this.Add(new Vector<TSize>(x, y), value);
            }
        }

        public new VectorDictionary<TSize, TValue> Add(Vector<TSize> key, TValue value)
        {
            if (!this.ContainsKey(key))
            {
                base.Add(key.Clone(), value);

                if (key.Y.ToLong() >= this.Height.ToLong())
                {
                    this.Height = (this.Height.ToLong() + 1).ToGeneric<TSize>();
                }

                this.Width = Math.Max(this.Width.ToLong(), key.X.ToLong()).ToGeneric<TSize>();
            }

            return this;
        }

        public void AddOrIncrement(Vector<TSize> key)
        {
            if (!this.ContainsKey(key))
            {
                base.Add(key.Clone(), 1.ToGeneric<TValue>());

                this.Height = (this.Height.ToLong() + 1).ToGeneric<TSize>();
                this.Width = Math.Max(this.Width.ToLong(), key.X.ToLong()).ToGeneric<TSize>();

                return;
            }

            this[key] = (this[key].ToLong() + 1).ToGeneric<TValue>();
        }

        public string Print(Func<TValue, char> comparer)
        {
            Vector<long> min = new(this.Min(x => x.Key.X).ToLong(), this.Min(x => x.Key.Y).ToLong());
            Vector<long> max = new(this.Max(x => x.Key.X).ToLong(), this.Max(x => x.Key.Y).ToLong());

            StringBuilder result = new();
            result.AppendLine().AppendLine();

            for (long y = min.Y; y <= max.Y; y++)
            {
                for (long x = min.Y; x <= max.X; x++)
                {
                    if (this.ContainsKey(new(x, y)))
                    {
                        result.Append(comparer(this[new(x, y)]));
                    }
                }

                result.AppendLine();
            }

            result.AppendLine();

            return result.ToString();
        }

        public VectorDictionary<TSize, TValue> Clone() => new(this);

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

        public bool IsVectorInRange(long x, long y)
            => x >= 0 && x < this.Width.ToLong() && y >= 0 && y < this.Height.ToLong();

        public bool IsVectorInRange(TSize x, TSize y)
            => x.ToLong() >= 0 && x.ToLong() < this.Width.ToLong() && y.ToLong() >= 0 && y.ToLong() < this.Height.ToLong();

        public bool IsVectorInRange(Vector<TSize> point)
            => point.X.ToLong() >= 0 && point.X.ToLong() < this.Width.ToLong() && point.Y.ToLong() >= 0 && point.Y.ToLong() < this.Height.ToLong();

        public bool IsVectorInRange((TSize X, TSize Y) point)
            => point.X.ToLong() >= 0 && point.X.ToLong() < this.Width.ToLong() && point.Y.ToLong() >= 0 && point.Y.ToLong() < this.Height.ToLong();

        public bool IsEdge(TValue value)
            => this.EdgeEnumerator().Any(x => x.Value?.Equals(value) ?? false);

        public bool IsEdge(Vector<TSize> point)
            => this.EdgeEnumerator().Any(x => x.Point == point);

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

        public IEnumerable<TValue?> Flatten()
        {
            foreach (VectorCell<TSize, TValue> cell in this.AxisEnumerator())
            {
                yield return cell.Value;
            }
        }

        public TValue Sum()
        {
            if (typeof(TValue) == typeof(int))
            {
                return this.SumInt();
            }

            return this.SumLong();
        }

        public BreadthFirstSearchResult<TSize> BreadthFirstSearch(Vector<TSize> source, Vector<TSize> destination, List<Vector<TSize>> blocked)
        {
            Queue<BreadthFirstSearchItem<TSize>> queue = new();

            bool[,] visited = new bool[this.Width.ToLong(), this.Height.ToLong()];
            visited[source.X.ToInt(), source.Y.ToInt()] = true;

            foreach (Vector<TSize> block in blocked)
            {
                visited[block.X.ToInt(), block.Y.ToInt()] = true;
            }

            queue.Enqueue(new(source, 0));

            while (queue.Count > 0)
            {
                BreadthFirstSearchItem<TSize> item = queue.Dequeue();

                if (item.Point == destination)
                {
                    return new(item.Path);
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

        public IEnumerable<VectorCell<TSize, TValue>> AxisEnumerator()
        {
            for (long y = 0; y < this.Height.ToLong(); y++)
            {
                for (long x = 0; x < this.Width.ToLong(); x++)
                {
                    Vector<TSize> key = new(x, y);

                    if (this.ContainsKey(key))
                    {
                        yield return new(key, this[new(key)]);

                        continue;
                    }

#pragma warning disable CS8604 // Possible null reference argument.
                    yield return new(key, default(TValue));
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
        }

        public IEnumerable<VectorCell<TSize, TValue>> AxisEnumerator(Action column)
        {
            for (int y = 0; y < this.Height.ToLong(); y++)
            {
                for (int x = 0; x < this.Width.ToLong(); x++)
                {
                    yield return new(new(x, y), this[y, x]);
                }

                column();
            }
        }

        public IEnumerable<List<VectorCell<TSize, TValue>>> RowEnumerator()
        {
            for (long y = 0; y < this.Height.ToLong(); y++)
            {
                yield return this.GetRow(y);
            }
        }

        public List<VectorCell<TSize, TValue>> GetRow(long y)
        {
            List<VectorCell<TSize, TValue>> result = new();

            for (long x = 0; x < this.Width.ToLong(); x++)
            {
                Vector<TSize> key = new(x, y);

                result.Add(new(key, this[key]));
            }

            return result;
        }

        public List<VectorCell<TSize, TValue>> GetColumn(int x)
        {
            List<VectorCell<TSize, TValue>> result = new();

            for (long y = 0; y < this.Height.ToLong(); y++)
            {
                Vector<TSize> key = new(x, y);

                result.Add(new(key, this[key]));
            }

            return result;
        }

        public void Resize(TSize width, TSize height)
        {
            this.Width = width;
            this.Height = height;

            foreach(KeyValuePair<Vector<TSize>, TValue> pair in this.ToList())
            {
                if(!this.IsVectorInRange(pair.Key))
                {
                    this.Remove(pair.Key);
                }
            }
        }

        private List<VectorCell<TSize, TValue>> Adjacent(List<VectorCell<TSize, TValue>> cells, Vector<TSize> point)
        {
            List<VectorCell<TSize, TValue>> result = new();

            foreach (VectorCell<TSize, TValue> cell in cells)
            {
                Vector<TSize> transformed = point + cell.Point;

                if (this.ContainsKey(transformed))
                {
                    result.Add(new VectorCell<TSize, TValue>(transformed, this[transformed], cell.Direction));
                }
            }

            return result;
        }

        private bool IsValidBreadthFirstSearch(Vector<TSize> point, bool[,] visited)
            => point.X.ToLong() >= 0 && point.Y.ToLong() >= 0 && point.X.ToLong() < this.Width.ToLong() && point.Y.ToLong() < this.Height.ToLong() && visited[point.X.ToLong(), point.Y.ToLong()] == false;

        private void Create(string[] input, Func<char, TValue?> comparer)
        {
            this.Width = input[0].Length.ToGeneric<TSize>();
            this.Height = input.Length.ToGeneric<TSize>();

            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    if (!this.ContainsKey(new(x, y)))
                    {
                        this.Add(new(x, y), comparer(input[y][x]) ?? (TValue)new object());
                    }
                }
            }
        }

        private void Create(string[] input, Func<char, TSize, TSize, TValue?> comparer)
        {
            this.Width = input[0].Length.ToGeneric<TSize>();
            this.Height = input.Length.ToGeneric<TSize>();

            for (int y = 0; y < this.Height.ToInt(); y++)
            {
                for (int x = 0; x < this.Width.ToInt(); x++)
                {
                    if (!this.ContainsKey(new(x, y)))
                    {
                        this.Add(new(x.ToGeneric<TSize>(), y.ToGeneric<TSize>()), comparer(input[y][x], x.ToGeneric<TSize>(), y.ToGeneric<TSize>()) ?? (TValue)new object());
                    }
                }
            }
        }

        private TValue SumInt() => (TValue)(object)this.AxisEnumerator().Sum(x => int.Parse($"{x.Value}"));

        private TValue SumLong() => (TValue)(object)this.AxisEnumerator().Sum(x => long.Parse($"{x.Value}"));
    }
}
