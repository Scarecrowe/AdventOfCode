namespace AdventOfCode.Puzzles._2020.Day_16___Ticket_Translation
{
    public class Ticket
    {
        public Ticket(string valuesString, Func<int, bool> valueValidator)
        {
            this.ValueValidator = valueValidator;
            this.Values = valuesString.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

        public List<int> Values { get; set; }

        private Func<int, bool> ValueValidator { get; }

        public List<int> GetErrorValues() => this.Values.Where(v => !this.ValueValidator(v)).ToList();

        public bool Validate() => !this.GetErrorValues().Any();
    }
}
