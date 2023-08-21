namespace AdventOfCode.Puzzles._2018.Day_16___Chronal_Classification
{
    public class ChronalClassification
    {
        public ChronalClassification(string[] input)
        {
            this.Samples = new();
            this.Program = new();
            int emptyLineCount = 0;

            int[] before = Array.Empty<int>();
            int[] after = Array.Empty<int>();
            int[] instruction = Array.Empty<int>();

            bool isProgram = false;

            foreach (string line in input)
            {
                if (isProgram)
                {
                    this.Program.Add(line.Split(" ").Select(c => int.Parse(c)).ToArray());
                    continue;
                }

                if (line.StartsWith("B"))
                {
                    before = line
                      .Replace("Before: [", string.Empty).Replace("]", string.Empty)
                      .Split(", ")
                      .Select(c => int.Parse(c))
                      .ToArray();
                }
                else if (line.StartsWith("A"))
                {
                    after = line
                       .Replace("After:  [", string.Empty)
                       .Replace("]", string.Empty)
                       .Split(", ")
                       .Select(c => int.Parse(c))
                       .ToArray();
                }
                else if (string.IsNullOrEmpty(line))
                {
                    emptyLineCount++;

                    if (emptyLineCount > 2)
                    {
                        isProgram = true;
                        this.Samples.RemoveAt(this.Samples.Count - 1);
                        continue;
                    }

                    this.Samples.Add(new Sample(before, after, instruction));

                    continue;
                }
                else
                {
                    instruction = line.Split(" ").Select(c => int.Parse(c)).ToArray();
                }

                emptyLineCount = 0;
            }

            this.OpCodes = this.OpCodes = new Dictionary<string, Action<int, int, int>>
                {
                    { "addr", this.Addr },
                    { "addi", this.Addi },
                    { "mulr", this.Mulr },
                    { "muli", this.Muli },
                    { "banr", this.Banr },
                    { "bani", this.Bani },
                    { "borr", this.Borr },
                    { "bori", this.Bori },
                    { "setr", this.Setr },
                    { "seti", this.Seti },
                    { "gtir", this.Gtir },
                    { "gtri", this.Gtri },
                    { "gtrr", this.Gtrr },
                    { "eqir", this.Eqir },
                    { "eqri", this.Eqri },
                    { "eqrr", this.Eqrr }
                };

            this.Registers = new int[4];
        }

        public List<Sample> Samples { get; }

        public Dictionary<string, Action<int, int, int>> OpCodes { get; private set; }

        public int[] Registers { get; private set; }

        public List<int[]> Program { get; }

        public List<List<string>> TestSamples()
        {
            List<List<string>> results = new();

            foreach (Sample test in this.Samples)
            {
                List<string> result = new();

                foreach (KeyValuePair<string, Action<int, int, int>> opCode in this.OpCodes)
                {
                    this.Registers = (int[])test.Before.Clone();
                    opCode.Value(test.Instruction[1], test.Instruction[2], test.Instruction[3]);

                    bool match = true;

                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Registers[i] != test.After[i])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        result.Add(opCode.Key);
                    }
                }

                results.Add(result);
            }

            return results;
        }

        public Dictionary<int, string> CalculateOpCodeIndexes(List<List<string>> results)
        {
            Dictionary<int, string> result = new();

            while (result.Count < 16)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].Count == 1)
                    {
                        if (!result.ContainsKey(this.Samples[i].Instruction[0]))
                        {
                            string opCode = results[i][0];
                            result.Add(this.Samples[i].Instruction[0], opCode);

                            for (int j = 0; j < results.Count; j++)
                            {
                                results[j].Remove(opCode);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public int Run()
        {
            Dictionary<int, string> opCodes = this.CalculateOpCodeIndexes(this.TestSamples());

            this.Registers = new int[4];

            foreach (int[] instruction in this.Program)
            {
                this.OpCodes[opCodes[instruction[0]]](instruction[1], instruction[2], instruction[3]);
            }

            return this.Registers[0];
        }

        private void Addr(int a, int b, int c) => this.Registers[c] = this.Registers[a] + this.Registers[b];

        private void Addi(int a, int b, int c) => this.Registers[c] = this.Registers[a] + b;

        private void Mulr(int a, int b, int c) => this.Registers[c] = this.Registers[a] * this.Registers[b];

        private void Muli(int a, int b, int c) => this.Registers[c] = this.Registers[a] * b;

        private void Banr(int a, int b, int c) => this.Registers[c] = this.Registers[a] & this.Registers[b];

        private void Bani(int a, int b, int c) => this.Registers[c] = this.Registers[a] & b;

        private void Borr(int a, int b, int c) => this.Registers[c] = this.Registers[a] | this.Registers[b];

        private void Bori(int a, int b, int c) => this.Registers[c] = this.Registers[a] | b;

        private void Setr(int a, int b, int c) => this.Registers[c] = this.Registers[a];

        private void Seti(int a, int b, int c) => this.Registers[c] = a;

        private void Gtir(int a, int b, int c) => this.Registers[c] = a > this.Registers[b] ? 1 : 0;

        private void Gtri(int a, int b, int c) => this.Registers[c] = this.Registers[a] > b ? 1 : 0;

        private void Gtrr(int a, int b, int c) => this.Registers[c] = this.Registers[a] > this.Registers[b] ? 1 : 0;

        private void Eqir(int a, int b, int c) => this.Registers[c] = a == this.Registers[b] ? 1 : 0;

        private void Eqri(int a, int b, int c) => this.Registers[c] = this.Registers[a] == b ? 1 : 0;

        private void Eqrr(int a, int b, int c) => this.Registers[c] = this.Registers[a] == this.Registers[b] ? 1 : 0;
    }
}
