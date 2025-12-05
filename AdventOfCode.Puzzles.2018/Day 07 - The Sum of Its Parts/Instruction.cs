namespace AdventOfCode.Puzzles._2018.Day_07___The_Sum_of_Its_Parts
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string line)
        {
            string[] tokens = line.Split(" must be finished before step ");
            this.Before = tokens[0].Remove("Step ")[0];
            this.After = tokens[1].Remove(" can begin.")[0];
        }

        public char Before { get; }

        public char After { get; }
    }
}