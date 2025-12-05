namespace AdventOfCode.Puzzles._2021.Day_23___Amphipod
{
    public class Amphipod
    {
        public Amphipod(string[] input, bool advanced = false) => this.Map = new(input, advanced);

        private AmphipodMap Map { get; }

        public int Run()
        {
            PriorityQueue<AmphipodMap, int> queue = new();
            queue.Enqueue(this.Map, this.Map.TotalEnergy);
            HashSet<string> visited = new();

            while (queue.Count > 0)
            {
                AmphipodMap map = queue.Dequeue();

                string key = map.ToKey();

                if (visited.Contains(key))
                {
                    continue;
                }

                visited.Add(key);

                if (map.IsComplete())
                {
                    return map.TotalEnergy;
                }

                map.ProcessCorridors(queue);
                map.ProcessRooms(queue);
            }

            throw new InvalidOperationException();
        }
    }
}
