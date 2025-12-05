namespace AdventOfCode.Puzzles._2023.Day_20___Pulse_Propagation
{
    public class PulsePropagation
    {
        private Dictionary<string, Module> Modules = new();
        private List<Module> FlipFlops = new();
        private string ButtonName = "button";
        private string RxTarget = "rx";

        public PulsePropagation(string[] input)
        {
            foreach (var line in input)
            {
                Module module = Module.FromInput(line);
                this.Modules[module.Name] = module;
            }

            foreach (var module in this.Modules.Values.ToList())
            {
                foreach (var destination in module.Destinations)
                {
                    if (!this.Modules.ContainsKey(destination))
                    {
                        this.Modules[destination] = new Module(destination, ModuleType.Output, Array.Empty<string>());
                    }
                }
            }

            this.Modules[ButtonName] = new Module(ButtonName, ModuleType.Button, new[] { "broadcaster" });

            foreach (var module in this.Modules.Values)
            {
                foreach (var destination in module.Destinations)
                {
                    if (this.Modules[destination].Type == ModuleType.Conjunction)
                    {
                        this.Modules[destination].Inputs[module.Name] = 0;
                    }
                }
            }

            this.FlipFlops = this.Modules.Values.Where(m => m.Type == ModuleType.FlipFlop).ToList();
        }

        public long LowAndHighMultiplied(int iterations = 1000)
        {
            long low = 0, high = 0;

            foreach (Module module in Modules.Values)
            {
                module.On = false;

                if (module.Type == ModuleType.Conjunction)
                {
                    foreach (var k in module.Inputs.Keys.ToList())
                    {
                        module.Inputs[k] = 0;
                    }
                }
            }

            for (int i = 0; i < iterations; i++)
            {
                Queue<Pulse> queue = new();
                queue.Enqueue(new Pulse(ButtonName, ButtonName, 0));

                while (queue.Count > 0)
                {
                    Pulse pulse = queue.Dequeue();
                    Module module = Modules[pulse.Target];
                    int outPulse = module.ReceivePulse(pulse.Source, pulse.Value);

                    if (outPulse == -1)
                    {
                        continue;
                    }

                    foreach (var dest in module.Destinations)
                    {
                        if (outPulse == 0)
                        {
                            low++;
                        }
                        else
                        {
                            high++;
                        }

                        queue.Enqueue(new Pulse(module.Name, dest, outPulse));
                    }
                }
            }

            return low * high;
        }

        public ulong FewestPresses()
        {
            int n = this.FlipFlops.Count;
            ulong presses = 0;

            var flipFlopIndices = this.FlipFlops.Select((m, i) => new { Module = m, Index = i }).ToArray();
            var topological = this.TopologicalOrder(RxTarget);

            foreach (var m in topological)
            {
                if (m.Type != ModuleType.FlipFlop)
                {
                    continue;
                }

                int idx = flipFlopIndices.First(f => f.Module == m).Index;
                presses += 1UL << idx;
            }

            return presses;
        }

        private List<Module> TopologicalOrder(string target)
        {
            HashSet<string> visited = new();
            List<Module>  order = new();

            void Visit(Module module)
            {
                if (!visited.Add(module.Name))
                {
                    return;
                }

                foreach (var src in Modules.Values.Where(x => x.Destinations.Contains(module.Name)))
                {
                    Visit(src);
                }

                order.Add(module);
            }

            Visit(Modules[target]);
            order.Reverse();

            return order;
        }
    }
}
