namespace AdventOfCode.Puzzles._2020.Day_16___Ticket_Translation
{
    using AdventOfCode.Core.Extensions;

    public class TicketTranslation
    {
        public TicketTranslation(string[] input) => this.Rules = Parse(input);

        public List<TicketRule> Rules { get; set; }

        public static int ErrorRate(string[] input)
        {
            Dictionary<string, HashSet<int>> fields = new();
            TicketMode mode = TicketMode.Fields;

            int invalid = 0;
            int count = 0;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    switch (mode)
                    {
                        case TicketMode.Fields:
                            mode = TicketMode.YourTicket;
                            break;
                        case TicketMode.YourTicket:
                            mode = TicketMode.NearbyTickets;
                            break;
                    }

                    continue;
                }

                switch (mode)
                {
                    case TicketMode.Fields:
                        string[] tokens = line.Split(":");
                        string[] ranges = tokens[1].Split("or");

                        HashSet<int> range = new();

                        for (int i = 0; i < ranges.Length; i++)
                        {
                            string[] numbers = ranges[i].Split("-");

                            int min = numbers[0].Trim().ToInt();
                            int max = numbers[1].Trim().ToInt();

                            for (int j = min; j <= max; j++)
                            {
                                range.Add(j);
                            }
                        }

                        fields.Add(tokens[0].Trim(), range);

                        break;
                    case TicketMode.YourTicket:

                        break;
                    case TicketMode.NearbyTickets:
                        if (line == "nearby tickets:")
                        {
                            continue;
                        }

                        string[] data = line.Split(",");

                        for (int i = 0; i < data.Length; i++)
                        {
                            int value = data[i].ToInt();
                            bool valid = false;

                            foreach (KeyValuePair<string, HashSet<int>> pair in fields)
                            {
                                if (pair.Value.Contains(value))
                                {
                                    valid = true;
                                    break;
                                }
                            }

                            if (!valid)
                            {
                                invalid += value;
                                count++;
                                break;
                            }
                        }

                        break;
                }
            }

            return invalid;
        }

        public static long Departures(string[] input)
        {
            List<List<string>> groups = input.ToList().Split(string.Empty).Select(l => l.ToList()).ToList();
            TicketTranslation ticket = new(groups[0].ToArray());
            List<Ticket> tickets = ticket.BuildTickets(groups[2]);
            List<Ticket> valid = tickets.Where(t => t.Validate()).ToList();
            Dictionary<int, string> lookup = ticket.RuleLookup(valid);

            IEnumerable<int> indexesToCheck = lookup
                .Where(l => l.Value.StartsWith("departure"))
                .Select(x => x.Key);

            Ticket yourTicket = ticket.BuildTicket(groups[1][1]);

            return indexesToCheck.Aggregate(1L, (a, i) => a * yourTicket.Values[i]);
        }

        public List<Ticket> BuildTickets(IList<string> inputs)
        {
            if (inputs[0].Equals("nearby tickets:"))
            {
                inputs = inputs.Skip(1).ToList();
            }

            return inputs.Select(this.BuildTicket).ToList();
        }

        public Ticket BuildTicket(string input) => new(input, this.ValidateTicket);

        public Dictionary<int, string> RuleLookup(List<Ticket> tickets)
        {
            List<List<int>> indexed = this.IndexedTicket(tickets);
            Dictionary<int, string> lookup = new();
            HashSet<TicketRule> available = this.AvailableTickets();
            HashSet<int> assigned = new();

            while (available.Any())
            {
                List<(int, TicketRule)> known = new();
                List<List<TicketRule>> possible = this.PossibleRules(indexed, available);

                for (int i = 0; i < possible.Count; i++)
                {
                    if (assigned.Contains(i))
                    {
                        continue;
                    }

                    if (possible[i].Count() == 1)
                    {
                        known.Add((i, possible[i].Single()));
                    }
                }

                foreach (var (i, rule) in known)
                {
                    assigned.Add(i);
                    available = available.Where(p => p.Name != rule.Name).ToHashSet();
                    lookup[i] = rule.Name;
                }
            }

            return lookup;
        }

        public bool ValidateTicket(int value) => this.Rules.Any(x => x.Validate(value));

        private static List<TicketRule> Parse(string[] input) => input.Select(x => new TicketRule(x)).ToList();

        private List<List<int>> IndexedTicket(List<Ticket> tickets)
        {
            return Enumerable.Range(0, this.Rules.Count)
                .Select(i =>
                    tickets.Select(t => t.Values[i]).ToList()).ToList();
        }

        private List<List<TicketRule>> PossibleRules(List<List<int>> indexes, HashSet<TicketRule> rules)
        {
            return indexes.Select(values => rules.Where(rule => values.All(rule.Validate)).ToList()).ToList();
        }

        private HashSet<TicketRule> AvailableTickets() => this.Rules.GetRange(0, this.Rules.Count).ToHashSet();
    }
}
