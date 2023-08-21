namespace AdventOfCode.Puzzles._2019.Day_07___Amplification_Circuit
{
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class AmplificationCircuit
    {
        public AmplificationCircuit(string[] input) => this.Program = input[0];

        private string Program { get; }

        public long HighestThrusterSignal()
        {
            List<List<int>> permutations = new List<int> { 0, 1, 2, 3, 4 }.Permutations(5);

            long result = 0;

            foreach (List<int> permutation in permutations)
            {
                long value = 0;
                IntcodeCpu cpu = new(this.Program);

                for (int i = 0; i < 5; i++)
                {
                    cpu.Reset();
                    cpu.Input.Enqueue(permutation[i]);
                    cpu.Input.Enqueue(value);
                    cpu.Run();
                    value = cpu.Output.Dequeue();
                }

                if (value > result)
                {
                    result = value;
                }
            }

            return result;
        }

        public long HighestFeedbackThrusterSignal()
        {
            List<IntcodeBus> buses = new();
            List<IntcodeCpu> cpus = new();

            for (int i = 0; i < 5; i++)
            {
                buses.Add(new());
            }

            List<List<int>> permutations = new List<int> { 5, 6, 7, 8, 9 }.Permutations(5);

            long result = 0;

            foreach (List<int> permutation in permutations)
            {
                cpus.Clear();
                cpus.Add(new(this.Program));
                cpus.Add(new(this.Program));
                cpus.Add(new(this.Program));
                cpus.Add(new(this.Program));
                cpus.Add(new(this.Program));

                long value = 0;
                IntcodeCpu cpu = new(this.Program);

                for(int i = 0; i < 5; i++)
                {
                    cpus[i].Input.Enqueue(permutation[i]);
                }

                int index = 0;

                while(true)
                {
                    cpus[index].Input.Enqueue(value);
                    cpus[index].Run();

                    if(!cpus[index].Output.Any())
                    {
                        break;
                    }

                    value = cpus[index].Output.Dequeue();
                    index = index.IncrementWrap(cpus.Count);
                }

                if (value > result)
                {
                    result = value;
                }
            }

            return result;
        }
    }
}
