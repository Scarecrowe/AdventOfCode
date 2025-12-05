namespace AdventOfCode.Puzzles._2019.IntCode
{
    using AdventOfCode.Core;

    public class IntcodeCpu
    {
        public IntcodeCpu(string program)
        {
            this.Memory = new(program);
            this.Program = program;
            this.RelativeBase = 0;
            this.Opcodes = new();
            this.Input = new();
            this.Output = new();

            this.BuildOpcodes();
        }

        public IntcodeCpu(IntcodeCpu cpu)
        {
            this.Memory = new(cpu.Memory.RAM);
            this.Program = cpu.Program;
            this.RelativeBase = cpu.RelativeBase;
            this.InstructionPointer = cpu.InstructionPointer;
            this.State = cpu.State;
            this.Opcodes = new();
            this.Input = new();
            this.Output = new();

            foreach (long value in cpu.Input)
            {
                this.Input.Enqueue(value);
            }

            foreach (long value in cpu.Output)
            {
                this.Output.Enqueue(value);
            }

            this.BuildOpcodes();
        }

        public IntcodeMemory Memory { get; private set; }

        public string Program { get; }

        public IntcodeBus Input { get; }

        public IntcodeBus Output { get; }

        public IntcodeCpuState State { get; private set; }

        private Dictionary<IntcodeOpcodeType, IntcodeOpcode> Opcodes { get; set; }

        private long InstructionPointer { get; set; }

        private long RelativeBase { get; set; }

        public static List<int> StringToAscii(string value)
        {
            List<int> result = new();

            foreach (char chr in value)
            {
                result.Add(chr);
            }

            result.Add(10);

            return result;
        }

        public IntcodeCpu Reset()
        {
            this.InstructionPointer = 0;
            this.RelativeBase = 0;
            this.Memory = new(this.Program);
            this.State = IntcodeCpuState.Idle;

            return this;
        }

        public IntcodeCpu Run()
        {
            this.State = IntcodeCpuState.Processing;

            IntcodeOpcode opcode = new((IntcodeOpcodeType)this.Memory.Read(this.InstructionPointer));

            while (opcode.Type != IntcodeOpcodeType.Terminate)
            {
                (this.Opcodes?[opcode.Type].Run)?.Invoke(opcode);

                if (this.State == IntcodeCpuState.Waiting)
                {
                    break;
                }

                opcode = new((IntcodeOpcodeType)this.Memory.Read(++this.InstructionPointer));
            }

            if (opcode.Type == IntcodeOpcodeType.Terminate)
            {
                this.State = IntcodeCpuState.Terminated;
            }

            return this;
        }

        public IntcodeCpu RunAsciiCommand(string command = "", bool print = false)
        {
            if (!string.IsNullOrEmpty(command))
            {
                this.Input.EnqueueRange(StringToAscii(command));
            }

            this.Run();

            if (print)
            {
                PuzzleConsole.Write(new string(this.Output.Select(c => (char)c).ToArray()));
                this.Output.Clear();
            }

            return this;
        }

        public IntcodeCpu Clone() => new(this);

        private long ReadValue(IntcodeOpcodeModeType mode)
        {
            return mode switch
            {
                IntcodeOpcodeModeType.Position => this.Memory.Read(this.Memory.Read(++this.InstructionPointer)),
                IntcodeOpcodeModeType.Immediate => this.Memory.Read(++this.InstructionPointer),
                IntcodeOpcodeModeType.Relative => this.Memory.Read(this.RelativeBase + this.Memory.Read(++this.InstructionPointer)),
                _ => throw new InvalidOperationException(),
            };
        }

        private void WriteValue(IntcodeOpcodeModeType mode, long value)
        {
            switch (mode)
            {
                case IntcodeOpcodeModeType.Position:
                    this.Memory.RAM[this.Memory.RAM[++this.InstructionPointer]] = value;
                    break;
                case IntcodeOpcodeModeType.Immediate:
                    this.Memory.RAM[++this.InstructionPointer] = value;
                    break;
                case IntcodeOpcodeModeType.Relative:
                    this.Memory.RAM[this.RelativeBase + this.Memory.RAM[++this.InstructionPointer]] = value;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void BuildOpcodes()
        {
            this.Opcodes = new()
            {
                // Add
                {
                    IntcodeOpcodeType.Add,
                    new IntcodeOpcode(IntcodeOpcodeType.Add, (opcode) =>
                    {
                        this.WriteValue(opcode.C, this.ReadValue(opcode.A) + this.ReadValue(opcode.B));
                    })
                },

                // Multiply
                {
                    IntcodeOpcodeType.Mulitply,
                    new IntcodeOpcode(IntcodeOpcodeType.Mulitply, (opcode) =>
                {
                    this.WriteValue(opcode.C, this.ReadValue(opcode.A) * this.ReadValue(opcode.B));
                })
                },

                // Input
                {
                    IntcodeOpcodeType.Input,
                    new IntcodeOpcode(IntcodeOpcodeType.Input, (opcode) =>
                {
                    while (this.Input.Count == 0)
                    {
                        this.State = IntcodeCpuState.Waiting;
                        return;
                    }

                    this.WriteValue(opcode.A, this.Input.Dequeue());
                })
                },

                // Output
                {
                    IntcodeOpcodeType.Output,
                    new IntcodeOpcode(IntcodeOpcodeType.Output, (opcode) =>
                {
                    this.Output.Enqueue(this.ReadValue(opcode.A));
                })
                },

                // Jump if true
                {
                    IntcodeOpcodeType.JumpIfTrue,
                    new IntcodeOpcode(IntcodeOpcodeType.JumpIfTrue, (opcode) =>
                {
                    long a = this.ReadValue(opcode.A);

                    if (a != 0)
                    {
                        this.InstructionPointer = this.ReadValue(opcode.B) - 1;
                        return;
                    }

                    this.InstructionPointer++;
                })
                },

                // Jump if false
                {
                    IntcodeOpcodeType.JumpIfFalse,
                    new IntcodeOpcode(IntcodeOpcodeType.JumpIfFalse, (opcode) =>
                {
                    long a = this.ReadValue(opcode.A);

                    if (a == 0)
                    {
                        this.InstructionPointer = this.ReadValue(opcode.B) - 1;
                        return;
                    }

                    this.InstructionPointer++;
                })
                },

                // Less than
                {
                    IntcodeOpcodeType.LessThan,
                    new IntcodeOpcode(IntcodeOpcodeType.LessThan, (opcode) =>
                {
                    this.WriteValue(opcode.C, this.ReadValue(opcode.A) < this.ReadValue(opcode.B) ? 1 : 0);
                })
                },

                // Equal
                {
                    IntcodeOpcodeType.Equal,
                    new IntcodeOpcode(IntcodeOpcodeType.Equal, (opcode) =>
                {
                    this.WriteValue(opcode.C, this.ReadValue(opcode.A) == this.ReadValue(opcode.B) ? 1 : 0);
                })
                },

                // Relative
                {
                    IntcodeOpcodeType.Relative,
                    new IntcodeOpcode(IntcodeOpcodeType.Relative, (opcode) =>
                {
                    this.RelativeBase += this.ReadValue(opcode.A);
                })
                },

                // Terminate
                {
                    IntcodeOpcodeType.Terminate,
                    new IntcodeOpcode(IntcodeOpcodeType.Terminate, (opcode) =>
                    {
                        this.State = IntcodeCpuState.Terminated;
                    })
                }
            };
        }
    }
}
