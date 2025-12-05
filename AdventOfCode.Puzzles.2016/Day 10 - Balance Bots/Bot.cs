namespace AdventOfCode.Puzzles._2016.Day_10___Balance_Bots
{
    public class Bot
    {
        public Bot(Instruction instruction)
        {
            this.LowIsBot = instruction.LowIsBot;
            this.HighIsBot = instruction.HighIsBot;
            this.LowIndex = instruction.LowIndex;
            this.HighIndex = instruction.HighIndex;
            this.Chips = new();
        }

        public bool LowIsBot { get; }

        public bool HighIsBot { get; }

        public int LowIndex { get; }

        public int HighIndex { get; }

        public List<int> Chips { get; private set; }

        public int Low() => this.Chips.Min();

        public int High() => this.Chips.Max();

        public void AddValue(int value) => this.Chips.Add(value);

        public void Process(BalanceBot balanceBot)
        {
            if (this.LowIsBot)
            {
                balanceBot.Bots[this.LowIndex].AddValue(this.Low());
            }
            else
            {
                if (balanceBot.Outputs.Count < this.LowIndex + 1)
                {
                    for (int i = balanceBot.Outputs.Count; i < this.LowIndex + 1; i++)
                    {
                        balanceBot.Outputs.Add(0);
                    }
                }

                balanceBot.Outputs[this.LowIndex] = this.Low();
            }

            if (this.HighIsBot)
            {
                balanceBot.Bots[this.HighIndex].AddValue(this.High());
            }
            else
            {
                if (balanceBot.Outputs.Count < this.HighIndex + 1)
                {
                    for (int i = balanceBot.Outputs.Count; i < this.HighIndex + 1; i++)
                    {
                        balanceBot.Outputs.Add(0);
                    }
                }

                balanceBot.Outputs[this.HighIndex] = this.High();
            }
        }
    }
}
