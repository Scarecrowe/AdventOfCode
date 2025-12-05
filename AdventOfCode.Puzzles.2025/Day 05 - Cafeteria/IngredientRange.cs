namespace AdventOfCode.Puzzles._2025.Day_05___Cafeteria
{
    public class IngredientRange
    {
        public long Start { get; set; }

        public long End { get; set; }

        public bool Contains(long value) => value >= this.Start && value <= this.End;
    }
}
