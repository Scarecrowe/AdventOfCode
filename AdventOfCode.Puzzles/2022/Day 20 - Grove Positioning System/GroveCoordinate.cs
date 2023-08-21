namespace AdventOfCode.Puzzles._2022.Day_20___Grove_Positioning_System
{
    public class GroveCoordinate
    {
        public GroveCoordinate(long value) => this.Value = value;

        public long Value { get; private set; }

        public GroveCoordinate? Next { get; private set; }

        public GroveCoordinate? Previous { get; private set; }

        public void SetPrevious(GroveCoordinate coordinate) => this.Previous = coordinate;

        public void SetNext(GroveCoordinate coordinate) => this.Next = coordinate;

        public void Decrypt(long key) => this.Value *= key;
    }
}
