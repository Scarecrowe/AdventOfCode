namespace AdventOfCode.Puzzles._2023.Day_07___Camel_Cards
{
    using AdventOfCode.Core.Extensions;

    public class Hand
    {
        public Hand(string value, string bid, string order)
        {
            this.Value = value;
            this.Strength = order.EndsWith('J') && value.Contains('J')
                ? order
                    .Where(x => x != 'J')
                    .Select(x => GetStrength(value.Replace('J', x)))
                    .Min()
                : GetStrength(value);
            this.Bid = int.Parse(bid);
            this.Ordered = this.Value.Select(x => 'A' + order.IndexOf(x)).ToStringX();
        }

        public string Value { get; }

        public string Ordered { get; }

        public int Bid { get; }

        public HandStrength Strength { get; }

        private static HandStrength GetStrength(string hand)
            => (HandStrength)Array.IndexOf(
                    new[] { "5", "41", "32", "311", "221", "2111", "11111" },
                    hand.GroupBy(x => x)
                        .OrderByDescending(x => x.Count())
                        .Select(x => x.Count())
                        .ToStringX());
    }
}
