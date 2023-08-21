namespace AdventOfCode.Puzzles._2019.Day_20___Donut_Maze
{
    using AdventOfCode.Core;

    public class ProcessedCell<TSize, TValue> : VectorCell<TSize, TValue>
    {
        public ProcessedCell(Vector<TSize> point, TValue value)
            : base(point, value)
        {
        }

        public ProcessedCell(VectorCell<TSize, TValue> cell)
            : base(cell.Point, cell.Value)
        {
        }

        public bool Processed { get; set; }
    }
}
