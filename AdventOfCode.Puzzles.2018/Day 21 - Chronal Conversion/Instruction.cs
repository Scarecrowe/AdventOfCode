namespace AdventOfCode.Puzzles._2018.Day_21___Chronal_Conversion
{
    using System.Text;

    public class Instruction
    {
        public Instruction(string line)
        {
            string[] tokens = line.Split(" ");

            this.Type = tokens[0];
            this.A = int.Parse(tokens[1]);
            this.B = int.Parse(tokens[2]);
            this.C = int.Parse(tokens[3]);
        }

        public Instruction(Instruction instruction)
        {
            this.Type = instruction.Type;
            this.A = instruction.A;
            this.B = instruction.B;
            this.C = instruction.C;
        }

        public string Type { get; }

        public int A { get; }

        public int B { get; }

        public int C { get; }

        public Instruction Clone() => new(this);

        public string Print(ChronalConversion cpu, bool programOnly = false)
        {
            int letter = (int)'A';
            StringBuilder instruction = new($"[{cpu.Program.IndexOf(this),2}] {this.Type} -> ");
            StringBuilder calculation = new();

            switch (this.Type)
            {
                case "addr":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} + {(char)(letter + this.B)}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} + {cpu.Registers[this.B]}");
                    break;
                case "addi":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} + {this.B}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} + {this.B}");
                    break;
                case "mulr":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} * {(char)(letter + this.B)}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} * {cpu.Registers[this.B]}");
                    break;
                case "muli":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} * {this.B}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} * {this.B}");
                    break;
                case "banr":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} & {(char)(letter + this.B)}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} & {cpu.Registers[this.B]}");
                    break;
                case "bani":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} & {this.B}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} & {this.B}");
                    break;
                case "borr":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} | {(char)(letter + this.B)}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} | {cpu.Registers[this.B]}");
                    break;
                case "bori":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)} | {this.B}");
                    calculation.Append($"{(char)(letter + this.C)} = {cpu.Registers[this.A]} | {this.B}");
                    break;
                case "setr":
                    instruction.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)}");
                    calculation.Append($"{(char)(letter + this.C)} = {(char)(letter + this.A)}");
                    break;
                case "seti":
                    instruction.Append($"{(char)(letter + this.C)} = {this.A}");
                    calculation.Append($"{(char)(letter + this.C)} = {this.A}");
                    break;
                case "gtir":
                    instruction.Append($"{(char)(letter + this.C)} = if {this.A} > {(char)(letter + this.B)} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(this.A > cpu.Registers[this.B] ? "1" : "0")}");
                    break;
                case "gtri":
                    instruction.Append($"{(char)(letter + this.C)} = if {(char)(letter + this.A)} > {this.B} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(cpu.Registers[this.A] > this.B ? "1" : "0")}");
                    break;
                case "gtrr":
                    instruction.Append($"{(char)(letter + this.C)} = if {(char)(letter + this.A)} > {(char)(letter + this.B)} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(cpu.Registers[this.A] > cpu.Registers[this.B] ? "1" : "0")}");
                    break;
                case "eqir":
                    instruction.Append($"{(char)(letter + this.C)} = if {this.A} == {(char)(letter + this.B)} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(this.A == cpu.Registers[this.B] ? "1" : "0")}");
                    break;
                case "eqri":
                    instruction.Append($"{(char)(letter + this.C)} = if {(char)(letter + this.A)} == {this.B} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(cpu.Registers[this.A] == this.B ? "1" : "0")}");
                    break;
                case "eqrr":
                    instruction.Append($"{(char)(letter + this.C)} = if {(char)(letter + this.A)} == {(char)(letter + this.B)} ? 1 : 0");
                    calculation.Append($"{(char)(letter + this.C)} = {(cpu.Registers[this.A] == cpu.Registers[this.B] ? "1" : "0")}");
                    break;
            }

            if (programOnly)
            {
                return $"{instruction,-40}";
            }

            return $"{instruction,-40}{calculation,-30}";
        }
    }
}
