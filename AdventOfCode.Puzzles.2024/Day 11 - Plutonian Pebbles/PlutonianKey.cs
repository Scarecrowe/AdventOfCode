namespace AdventOfCode.Puzzles._2024.Day_11___Plutonian_Pebbles
{
    public readonly struct PlutonianKey(long number, int count) : IEquatable<PlutonianKey>
    {
        public long Stone { get; } = number;

        public int Count { get; } = count;

        public override readonly bool Equals(object obj) => obj is PlutonianKey other && Equals(other);

        public readonly bool Equals(PlutonianKey other) => this.Stone == other.Stone && this.Count == other.Count;

        public override readonly int GetHashCode() => HashCode.Combine(this.Stone, this.Count);

        public static bool operator ==(PlutonianKey left, PlutonianKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlutonianKey left, PlutonianKey right)
        {
            return !(left == right);
        }
    }
}
