namespace AdventOfCode.Puzzles._2024.Day_05___Print_Queue
{
    public class PrintQueue
    {
        public Dictionary<int, HashSet<int>> Rules { get; private set; }

        public PrintQueue()
        {
            this.Rules = new Dictionary<int, HashSet<int>>();
        }
    }
}
