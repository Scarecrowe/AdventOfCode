namespace AdventOfCode.Puzzles._2023.Day_20___Pulse_Propagation
{
    public enum ModuleType
    {
        FlipFlop,
        Conjunction,
        Broadcaster,
        Button,
        Output
    }

    public class Module
    {
        public string Name { get; }

        public ModuleType Type { get; }

        public List<string> Destinations { get; }

        public bool On { get; set; } = false;

        public Dictionary<string, int> Inputs { get; } = new();

        public Module(string name, ModuleType type, IEnumerable<string> dests)
        {
            this.Name = name;
            this.Type = type;
            this.Destinations = dests.ToList();
        }

        public static Module FromInput(string line)
        {
            var parts = line.Split(" -> ");
            var left = parts[0];
            var dests = parts[1].Split(", ");

            if (left.StartsWith("%"))
            {
                return new Module(left[1..], ModuleType.FlipFlop, dests);
            }

            if (left.StartsWith("&"))
            {
                return new Module(left[1..], ModuleType.Conjunction, dests);
            }

            return new Module(left, ModuleType.Broadcaster, dests);
        }

        public int ReceivePulse(string from, int pulse)
        {
            switch (this.Type)
            {
                case ModuleType.Button:
                case ModuleType.Broadcaster:
                    return pulse;

                case ModuleType.FlipFlop:
                    if (pulse == 1)
                    {
                        return -1;
                    }

                    this.On = !this.On;
                    return this.On ? 1 : 0;

                case ModuleType.Conjunction:
                    this.Inputs[from] = pulse;
                    bool allHigh = this.Inputs.Values.All(v => v == 1);
                    return allHigh ? 0 : 1;

                default:
                    return pulse;
            }
        }
    }
}
