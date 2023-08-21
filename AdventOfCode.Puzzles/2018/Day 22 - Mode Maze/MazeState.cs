namespace AdventOfCode.Puzzles._2018.Day_22___Mode_Maze
{
    using AdventOfCode.Core;

    public class MazeState
    {
        public MazeState(Vector<long> point, MazeToolType tool, long switchCount, long moveCount)
        {
            this.Point = point;
            this.Tool = tool;
            this.MoveCount = ++moveCount;
            this.SwitchCount = switchCount;
        }

        public Vector<long> Point { get; }

        public MazeToolType Tool { get; }

        public long SwitchCount { get; }

        public long MoveCount { get; }

        public long Score() => (this.SwitchCount * 7) + this.MoveCount;
    }
}
