namespace AdventOfCode.Puzzles._2023.Day_07___Camel_Cards
{
    using AdventOfCode.Core.Extensions;

    public class CamelCards
    {
        public CamelCards(string[] input, bool wildcards = false)
            => this.Hands = Parse(!wildcards ? "AKQJT98765432" : "AKQT98765432J", input);

        private Dictionary<string, Hand> Hands { get; }

        public long Play()
            => this.Hands
                .OrderBy(x => x.Value.Strength)
                .ThenBy(x => x.Value.Ordered)
                .Reverse()
                .Select((x, i) => x.Value.Bid * (i + 1))
                .Sum();

        private static Dictionary<string, Hand> Parse(string order, string[] input)
            => input
            .Select(x => x.Split(" "))
            .ToDictionary(x => x[0], x => new Hand(x[0], x[1], order));
    }
}
