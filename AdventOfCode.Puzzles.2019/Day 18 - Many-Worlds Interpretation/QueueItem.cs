namespace AdventOfCode.Puzzles._2019.Day_18___Many_Worlds_Interpretation
{
    using AdventOfCode.Core;

    public class QueueItem
    {
        public QueueItem(Vector<int> location, long distance, bool[] visited)
        {
            this.Location = location;
            this.Distance = distance;
            this.Visited = visited;
        }

        public Vector<int> Location { get; }

        public long Distance { get; }

        public bool[] Visited { get; set; }
    }
}
