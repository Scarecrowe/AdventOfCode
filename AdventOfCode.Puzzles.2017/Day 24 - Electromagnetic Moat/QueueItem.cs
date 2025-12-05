namespace AdventOfCode.Puzzles._2017.Day_24___Electromagnetic_Moat
{
    public class QueueItem
    {
        public QueueItem(Port port, List<Port> ports, int strength, int connectingPort, int length)
        {
            this.Port = port;
            this.Ports = ports;
            this.Strength = strength;
            this.ConnectingPort = connectingPort;
            this.Length = length;
        }

        public List<Port> Ports { get; }

        public Port Port { get; }

        public int Strength { get; private set; }

        public int ConnectingPort { get; }

        public int Length { get; }
    }
}
