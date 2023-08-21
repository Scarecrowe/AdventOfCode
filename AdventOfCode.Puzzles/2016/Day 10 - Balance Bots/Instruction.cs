namespace AdventOfCode.Puzzles._2016.Day_10___Balance_Bots
{
    using AdventOfCode.Core.Extensions;

    public class Instruction
    {
        public Instruction(string instruction)
        {
            string[] tokens;

            if (instruction.StartsWith("value"))
            {
                this.IsDefault = true;

                tokens = instruction
                    .Replace("value ")
                    .Split(" goes to bot ", StringSplitOptions.RemoveEmptyEntries);

                this.Value = tokens[0].ToInt();
                this.Bot = tokens[1].ToInt();

                return;
            }

            tokens = instruction.Split(" gives low to ", StringSplitOptions.RemoveEmptyEntries);

            this.Bot = tokens[0].Replace("bot ").ToInt();

            tokens = tokens[1].Split(" and high to ", StringSplitOptions.RemoveEmptyEntries);

            this.LowIsBot = tokens[0].StartsWith("bot");
            this.LowIndex = tokens[0].Replace("bot ").Replace("output ").ToInt();

            this.HighIsBot = tokens[1].StartsWith("bot");
            this.HighIndex = tokens[1].Replace("bot ").Replace("output ").ToInt();
        }

        public bool IsDefault { get; }

        public int Value { get; }

        public int Bot { get; }

        public bool LowIsBot { get; }

        public bool HighIsBot { get; }

        public int LowIndex { get; }

        public int HighIndex { get; }
    }
}
