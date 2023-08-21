namespace AdventOfCode.Core
{
    using AdventOfCode.Core.Contracts;

    public class NegativeIndexArray<TValue>
    {
        public NegativeIndexArray(int capacity, int min)
        {
            capacity.Should().Not().BeLessThanZero(paramName: nameof(capacity));
            min.Should().BeLessThanZero(paramName: nameof(min));

            this.Values = NegativeIndexArray<TValue>.CreateArray(capacity);
            this.Min = min;
            this.Max = capacity + (min * -1);
        }

        public int Min { get; private set; }

        public int Max { get; private set; }

        public int Length => this.Values.Length;

        public TValue[] Values { get; }

        public TValue this[int index]
        {
            get
            {
                index += this.Min * -1;

                return this.Values[index];
            }

            set
            {
                index += this.Min * -1;

                this.Values[index] = value;
            }
        }

        public bool HasIndex(int index)
        {
            if (index < this.Min)
            {
                return false;
            }

            if (index + (this.Min * -1) >= this.Values.Length)
            {
                return false;
            }

            return true;
        }

        private static TValue[] CreateArray(int capacity)
            => Array.CreateInstance(typeof(TValue), capacity).OfType<TValue>().ToArray();
    }
}
