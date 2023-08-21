namespace AdventOfCode.Puzzles._2017.Day_25___The_Halting_Problem
{
    using AdventOfCode.Core.Extensions;

    public class TuringMachine
    {
        public TuringMachine(string[] input)
        {
            this.Cursor = 0;
            this.Tape = new();
            this.States = ParseStates(input);
            this.State = input[0].Replace("Begin in state ").Replace(".")[0];
            this.ChecksumStep = input[1].Replace("Perform a diagnostic checksum after ").Replace(" steps.").ToInt();
        }

        public Dictionary<char, State> States { get; }

        private Dictionary<int, int> Tape { get; }

        private char State { get; set; }

        private int Cursor { get; set; }

        private int ChecksumStep { get; }

        public TuringMachine Run()
        {
            for (int i = 1; i <= this.ChecksumStep; i++)
            {
                this.RunStep();
            }

            return this;
        }

        public int CountOnes() => this.Tape.Sum(x => x.Value);

        private static Dictionary<char, State> ParseStates(string[] input)
        {
            Dictionary<char, State> result = new();

            char letter = '\0';
            int number = 0;
            int[] values = new int[2];
            Direction[] directions = new Direction[2];
            char[] states = new char[2];

            for (int i = 2; i < input.Length; i++)
            {
                if (input[i].StartsWith("In state"))
                {
                    letter = input[i].Replace("In state ").Replace(":")[0];
                    continue;
                }
                else if (input[i].StartsWith("  If the current value is"))
                {
                    number = input[i].Replace("  If the current value is ").Replace(":").ToInt();
                    continue;
                }
                else if (input[i].StartsWith("    - Write the value"))
                {
                    values[number] = input[i].Replace("    - Write the value ").Replace(".").ToInt();
                    continue;
                }
                else if (input[i].StartsWith("    - Move one slot to the"))
                {
                    directions[number] = input[i].Replace("    - Move one slot to the ").Replace(".") == "left"
                        ? Direction.Left
                        : Direction.Right;
                    continue;
                }
                else if (input[i].StartsWith("    - Continue with state"))
                {
                    states[number] = input[i].Replace("    - Continue with state ").Replace(".")[0];
                    continue;
                }
                else
                {
                    result.Add(letter, new(letter, (int[])values.Clone(), (Direction[])directions.Clone(), (char[])states.Clone()));
                }
            }

            result.Add(letter, new(letter, (int[])values.Clone(), (Direction[])directions.Clone(), (char[])states.Clone()));

            return result;
        }

        private void RunStep()
        {
            if (!this.Tape.ContainsKey(this.Cursor))
            {
                this.Tape.Add(this.Cursor, 0);
            }

            int value = this.Tape[this.Cursor];

            this.Tape[this.Cursor] = this.States[this.State].Values[value];

            this.Cursor += this.States[this.State].Directions[value] == Direction.Left ? -1 : 1;
            this.State = this.States[this.State].States[value];
        }
    }
}
