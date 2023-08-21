namespace AdventOfCode.Puzzles._2019.IntCode
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class IntcodeOpcode
    {
        public IntcodeOpcode(IntcodeOpcodeType type)
        {
            this.Value = (int)type;

            if (this.Value > 99)
            {
                string number = $"{this.Value}".PadLeft(5, '0');

                this.Type = (IntcodeOpcodeType)$"{number[3]}{number[4]}".ToInt();
                this.A = (IntcodeOpcodeModeType)number[2].ToString().ToInt();
                this.B = (IntcodeOpcodeModeType)number[1].ToString().ToInt();
                this.C = (IntcodeOpcodeModeType)number[0].ToString().ToInt();
            }
            else
            {
                this.Type = type;
            }

            switch (this.Type)
            {
                case IntcodeOpcodeType.Add:
                case IntcodeOpcodeType.Mulitply:
                case IntcodeOpcodeType.LessThan:
                case IntcodeOpcodeType.Equal:
                    this.ParameterCount = 3;
                    break;
                case IntcodeOpcodeType.JumpIfTrue:
                case IntcodeOpcodeType.JumpIfFalse:
                    this.ParameterCount = 2;
                    break;
                case IntcodeOpcodeType.Input:
                case IntcodeOpcodeType.Output:
                case IntcodeOpcodeType.Relative:
                    this.ParameterCount = 1;
                    break;
                case IntcodeOpcodeType.Terminate:
                    this.ParameterCount = 0;
                    break;
                default:
                    this.Type = IntcodeOpcodeType.None;
                    break;
            }
        }

        public IntcodeOpcode(IntcodeOpcodeType type, Action<IntcodeOpcode> action)
            : this(type) => this.Run = action;

        public IntcodeOpcodeType Type { get; }

        public int Value { get; }

        public IntcodeOpcodeModeType A { get; }

        public IntcodeOpcodeModeType B { get; }

        public IntcodeOpcodeModeType C { get; }

        public int ParameterCount { get; }

        public Action<IntcodeOpcode>? Run { get; }

        public new string ToString() => $"{this.Value,-6}{this.Type,-12}{(this.Type == IntcodeOpcodeType.None ? string.Empty : this.OpcodeModesToString()),-4}";

        private static string OpcodeModeToString(IntcodeOpcodeModeType modeType) => modeType.ToString()[0].ToString();

        private string OpcodeModesToString()
        {
            StringBuilder result = new();

            switch (this.ParameterCount)
            {
                case 0:
                    break;
                case 1:
                    result.Append(OpcodeModeToString(this.A));
                    break;
                case 2:
                    result.Append(OpcodeModeToString(this.A));
                    result.Append(OpcodeModeToString(this.B));
                    break;
                case 3:
                    result.Append(OpcodeModeToString(this.A));
                    result.Append(OpcodeModeToString(this.B));
                    result.Append(OpcodeModeToString(this.C));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return result.ToString();
        }
    }
}
