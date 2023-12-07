namespace AdventOfCode.Puzzles._2023.Day_07___Camel_Cards
{
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
            this.Ordered = string.Join(string.Empty, this.Value.Select(y => 'A' + order.IndexOf(y)));
        }

        public string Value { get; }

        public int Bid { get; }

        public HandStrength Strength { get; }

        public string Ordered { get; }

        private static HandStrength GetStrength(string hand)
        {
            Dictionary<char, int> cards = hand.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            int max = cards.Max(x => x.Value);

            if (max == 5)
            {
                return HandStrength.FiveOfAKind;
            }
            else if (max == 4)
            {
                return HandStrength.FourOfAKind;
            }
            else if (max == 3)
            {
                return cards.Count == 2 ? HandStrength.FullHouse : HandStrength.ThreeOfAKind;
            }
            else if (cards.Count == 3 && max == 2 && cards.Min(x => x.Value) == 1)
            {
                return HandStrength.TwoPair;
            }

            return cards.Count == 5 ? HandStrength.HighCard : HandStrength.OnePair;
        }
    }
}
