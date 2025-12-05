namespace AdventOfCode.Puzzles._2022.Day_16___Proboscidea_Volcanium
{
    public class Valve
    {
        public Valve(string name, int flowRate)
        {
            this.Name = name;
            this.FlowRate = flowRate;
            this.Tunnels = new();
            this.Paths = new();
            this.Closed = true;
            this.Visited = false;
        }

        public Valve(Valve valve)
        {
            this.Name = valve.Name;
            this.FlowRate = valve.FlowRate;
            this.Closed = valve.Closed;
            this.Tunnels = new();
            this.Paths = new();
            this.Visited = valve.Visited;
        }

        public string Name { get; private set; }

        public int FlowRate { get; private set; }

        public List<string> Tunnels { get; private set; }

        public Dictionary<string, int> Paths { get; private set; }

        public bool Closed { get; private set; }

        public bool Visited { get; private set; }

        public void Open() => this.Closed = false;

        public void Visit() => this.Visited = true;
    }
}
