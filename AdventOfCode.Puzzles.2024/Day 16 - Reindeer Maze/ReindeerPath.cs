namespace AdventOfCode.Puzzles._2024.Day_16___Reindeer_Maze
{
    using AdventOfCode.Core;

    public class ReindeerPath
    {
        public HashSet<(Vector<int> Point, int Score)> Points { get; set; }

        public HashSet<Vector<int>> Visited { get; set; }

        public int Score { get; set; }

        public ReindeerPath(int score)
        {
            this.Points = [];
            this.Visited = [];
            this.Score = score;
        }

        public ReindeerPath Clone()
        {
            ReindeerPath result = new(this.Score);
            result.Points = [.. this.Points];

            return result;
        }
    }
}
