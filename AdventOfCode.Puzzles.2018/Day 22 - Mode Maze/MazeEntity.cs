namespace AdventOfCode.Puzzles._2018.Day_22___Mode_Maze
{
    public class MazeEntity
    {
        public MazeEntity(MazeEntityType region, long errorsionLevel)
        {
            this.Region = region;
            this.ErrosionLevel = errorsionLevel;
        }

        public MazeEntityType Region { get; }

        public long ErrosionLevel { get; }
    }
}
