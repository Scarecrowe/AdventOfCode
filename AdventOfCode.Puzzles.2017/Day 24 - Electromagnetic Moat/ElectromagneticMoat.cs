namespace AdventOfCode.Puzzles._2017.Day_24___Electromagnetic_Moat
{
    public class ElectromagneticMoat
    {
        public ElectromagneticMoat(string[] input) => this.Ports = Parse(input);

        public List<Port> Ports { get; }

        public int Strongest { get; private set; }

        public int Longest { get; private set; }

        public ElectromagneticMoat BuildBridges()
        {
            this.Strongest = 0;
            Dictionary<int, List<int>> lengths = new();

            List<Port> starts = this.Ports.Where(x => x.Left == 0 || x.Right == 0).ToList();

            Queue<QueueItem> queue = new();

            foreach (Port start in starts)
            {
                List<Port> clone = this.Ports.Select(x => x).ToList();
                clone.Remove(start);
                queue.Enqueue(new QueueItem(start, clone, start.Left + start.Right, start.Left == 0 ? start.Right : start.Left, 1));
            }

            while (queue.Count > 0)
            {
                QueueItem item = queue.Dequeue();

                List<Port> available = item.Ports.Where(x => x.Left == item.ConnectingPort || x.Right == item.ConnectingPort).ToList();

                if (available.Count == 0)
                {
                    this.Strongest = Math.Max(this.Strongest, item.Strength);

                    if (!lengths.ContainsKey(item.Length))
                    {
                        lengths.Add(item.Length, new());
                    }

                    lengths[item.Length].Add(item.Strength);
                }

                foreach (Port port in available)
                {
                    List<Port> clone = item.Ports.Select(x => x).ToList();
                    clone.Remove(port);

                    queue.Enqueue(new QueueItem(port, clone, item.Strength + (port.Left + port.Right), port.Left == item.ConnectingPort ? port.Right : port.Left, item.Length + 1));
                }
            }

            this.Longest = lengths[lengths.Max(x => x.Key)].Max();

            return this;
        }

        private static List<Port> Parse(string[] input) => input.Select(x => new Port(x)).ToList();
    }
}
