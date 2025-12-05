namespace AdventOfCode.Puzzles._2024.Day_20___Race_Condition
{
    using AdventOfCode.Core;

    public class RaceState
    {
        public Vector<int> Point { get; set; }

        public HashSet<Vector<int>> Path { get; private set; }  

        public bool Cheated { get; set; }

        public RaceState(Vector<int> point)
        {
            this.Point = point;
            this.Path = [];
        }

        public RaceState(Vector<int> point, HashSet<Vector<int>> path, bool cheated)
        {
            this.Point = point;
            this.Path = path.ToHashSet();
            this.Cheated = cheated;
        }

        public RaceState Clone() => new RaceState(this.Point, this.Path, this.Cheated);
    }
}
