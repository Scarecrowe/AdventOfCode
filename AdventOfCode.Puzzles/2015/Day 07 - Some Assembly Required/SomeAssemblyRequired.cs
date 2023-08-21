namespace AdventOfCode.Puzzles._2015.Day_07___Some_Assembly_Required
{
    using AdventOfCode.Core;

    public class SomeAssemblyRequired
    {
        public SomeAssemblyRequired(string[] input)
        {
            this.Wires = new();
            this.Instructions = new();

            for (int i = 0; i < input.Length; i++)
            {
                this.Instructions.Add(i, new(input[i]));
            }

            this.OriginalInstructions = this.Instructions.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, ushort> Wires { get; private set; }

        public Dictionary<int, Instruction> Instructions { get; private set; }

        public Dictionary<int, Instruction> OriginalInstructions { get; }

        public Dictionary<int, Instruction> Setters() => this.Instructions
            .Where(x => x.Value.IsSetter())
            .ToDictionary(x => x.Key, x => x.Value);

        public void Process(Instruction instruction)
        {
            int inputA = instruction.InputA.IsAddress ? this.Wires[instruction.InputA.Address] : instruction.InputA.Constant;
            int inputB = 0;

            if (instruction.InputB != null)
            {
                inputB = instruction.InputB.IsAddress
                    ? this.Wires[instruction.InputB.Address]
                    : instruction.InputB.Constant;
            }

            switch (instruction.LogicGate)
            {
                case Bitwise.Not:
                    this.Wires[instruction.Output.Address] = (ushort)(~inputA);
                    break;
                case Bitwise.And:
                    this.Wires[instruction.Output.Address] = (ushort)(inputA & inputB);
                    break;
                case Bitwise.Or:
                    this.Wires[instruction.Output.Address] = (ushort)(inputA | inputB);
                    break;
                case Bitwise.RShift:
                    this.Wires[instruction.Output.Address] = (ushort)(inputA >> inputB);
                    break;
                case Bitwise.LShift:
                    this.Wires[instruction.Output.Address] = (ushort)(inputA << inputB);
                    break;
                case Bitwise.Set:
                    this.Wires[instruction.Output.Address] = (ushort)inputA;
                    break;
            }
        }

        public SomeAssemblyRequired Assemble()
        {
            Dictionary<int, Instruction> setters = this.Setters();

            foreach (KeyValuePair<int, Instruction> pair in setters)
            {
                this.Wires[pair.Value.Output.Address] = pair.Value.InputA.Constant;
                this.Instructions.Remove(pair.Key);
            }

            while (this.Instructions.Count > 1)
            {
                Dictionary<int, Instruction> instructions = this.Instructions
                    .Where(pair => ((pair.Value.InputA.IsAddress && this.Wires.ContainsKey(pair.Value.InputA.Address))
                        || !pair.Value.InputA.IsAddress)
                        && ((pair.Value.InputB?.IsAddress == true && this.Wires.ContainsKey(pair.Value.InputB.Address))
                        || pair.Value.InputB?.IsAddress == false
                        || (pair.Value.InputB == null && pair.Value.LogicGate == Bitwise.Not)))
                    .ToDictionary(x => x.Key, x => x.Value);

                foreach (KeyValuePair<int, Instruction> pair in instructions)
                {
                    this.Process(pair.Value);
                    this.Instructions.Remove(pair.Key);
                }
            }

            if (this.Instructions.Count > 0)
            {
                this.Process(this.Instructions.ElementAt(0).Value);
            }

            return this;
        }

        public SomeAssemblyRequired Reassemble()
        {
            ushort signal = this.Wires["a"];
            string address = "b";

            this.Wires = new();
            this.Instructions = this.OriginalInstructions.ToDictionary(x => x.Key, x => x.Value);

            int key = this.Instructions.FirstOrDefault(x => (x.Value.Output?.Address ?? string.Empty) == address).Key;

            if (this.Instructions.ContainsKey(key))
            {
                this.Instructions[key].InputA.SetConstant(signal);
                this.Assemble();
            }

            return this;
        }

        public int WireA() => this.Wires["a"];
    }
}
