namespace AdventOfCode.Puzzles._2018.Day_22___Mode_Maze
{
    public class MazeMove
    {
        public MazeMove(MazeToolType tool, int switchCount)
        {
            this.Tool = tool;
            this.SwitchCount = switchCount;
        }

        public MazeToolType Tool { get; }

        public long SwitchCount { get; }
    }
}
