namespace AdventOfCode.Puzzles._2020.Day_16___Ticket_Translation
{
    using AdventOfCode.Core.Extensions;

    public class TicketRule
    {
        public TicketRule(string input)
        {
            this.Name = input.Split(":")[0];
            string rest = input.Split(":")[1].Trim();
            string rangeA = rest.Split(" or ")[0];
            string rangeB = rest.Split(" or ")[1];

            this.MinA = rangeA.Split("-")[0].ToInt();
            this.MaxA = rangeA.Split("-")[1].ToInt();
            this.MinB = rangeB.Split("-")[0].ToInt();
            this.MaxB = rangeB.Split("-")[1].ToInt();
        }

        public string Name { get; }

        public int MinA { get; }

        public int MaxA { get; }

        public int MinB { get; }

        public int MaxB { get; }

        public bool Validate(int value) => (this.MinA <= value && this.MaxA >= value) || (this.MinB <= value && this.MaxB >= value);
    }
}
