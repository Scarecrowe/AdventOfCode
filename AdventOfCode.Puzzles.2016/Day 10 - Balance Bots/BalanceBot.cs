namespace AdventOfCode.Puzzles._2016.Day_10___Balance_Bots
{
    using AdventOfCode.Core.Extensions;

    public class BalanceBot
    {
        public BalanceBot(string[] input)
        {
            this.Instructions = new();
            this.Bots = new();
            this.Outputs = new();
            this.SetupInstructions = new();
            this.LoadInstructions = new();

            this.ParseInput(input);
        }

        public HashSet<Instruction> Instructions { get; private set; }

        public Dictionary<int, Bot> Bots { get; private set; }

        public List<int> Outputs { get; private set; }

        private HashSet<Instruction> SetupInstructions { get; set; }

        private HashSet<Instruction> LoadInstructions { get; set; }

        public BalanceBot Process()
        {
            List<int> complete = new();
            List<int> process = new() { this.LoadInstructions.GroupBy(x => x.Bot).First(x => x.Count() > 1).Key };

            while (process.Count > 0)
            {
                foreach (int bot in process)
                {
                    this.Bots[bot].Process(this);

                    complete.Add(bot);
                }

                process = this.Bots
                    .Where(x => x.Value.Chips.Count > 1 && !complete.Contains(x.Key))
                    .Select(x => x.Key)
                    .ToList();
            }

            return this;
        }

        public int ComparerBot(int low, int high)
        {
            List<int> values = new() { low, high };

            return this.Bots.Where(x => x.Value.Chips.Where(y => values.Contains(y)).Count() > 1).First().Key;
        }

        public long MultipleOutputs()
        {
            return this.Outputs[0] * this.Outputs[1] * this.Outputs[2];
        }

        private void ParseInput(string[] input)
        {
            this.Instructions = input.Select(x => new Instruction(x)).ToHashSet();
            this.SetupInstructions = this.Instructions.Where(x => !x.IsDefault).ToHashSet();
            this.LoadInstructions = this.Instructions.Where(x => x.IsDefault).ToHashSet();
            this.SetupInstructions.ForEach(x => this.Bots.Add(x.Bot, new Bot(x)));
            this.LoadInstructions.ForEach(x => this.Bots[x.Bot].AddValue(x.Value));
        }
    }
}
