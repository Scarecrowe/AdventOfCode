namespace AdventOfCode.Puzzles._2023.Day_17___Clumsy_Crucible
{
    using AdventOfCode.Core;

    public class State
    {
        public State(Vector<int> point, Cardinal direction, int distance)
        {
            this.Point = point;
            this.Direction = direction;
            this.Distance = distance;
        }

        public Vector<int> Point { get; }

        public Cardinal Direction { get; }

        public int Distance { get; }

        public override int GetHashCode()
            => (this.Point, this.Direction, this.Distance).GetHashCode();

        public bool Equals(State? state)
            => state?.GetHashCode() == this.GetHashCode();

        public override bool Equals(object? obj)
            => this.Equals((State)(obj ?? new()));

        public State Next(VectorCell<int, int> cell)
            => new(cell.Point, cell.Direction, cell.Direction == this.Direction ? this.Distance + 1 : 1);
    }
}
