namespace AdventOfCode.Puzzles._2021.Day_22___Reactor_Reboot
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string line)
        {
            this.Min = new();
            this.Max = new();

            string[] tokens = line.SplitSpace();

            this.IsOn = tokens[0] == "on";

            tokens = tokens[1].Split(",");
            long[] numbers = tokens[0][2..].Split("..").ToLong();

            this.Min.X = numbers[0];
            this.Max.X = numbers[1];

            numbers = tokens[1][2..].Split("..").ToLong();
            this.Min.Y = numbers[0];
            this.Max.Y = numbers[1];

            numbers = tokens[2][2..].Split("..").ToLong();

            this.Min.Z = numbers[0];
            this.Max.Z = numbers[1];

            this.Ignore = !(this.Min.X > -50 && this.Max.X < 50 && this.Min.Y > -50 && this.Max.Y < 50 && this.Min.Z > -50 && this.Max.Z < 50);
        }

        public bool IsOn { get; }

        public Vector<long> Min { get; }

        public Vector<long> Max { get; }

        public bool Ignore { get; }
    }
}
