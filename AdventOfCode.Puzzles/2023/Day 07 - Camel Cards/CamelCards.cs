namespace AdventOfCode.Puzzles._2023.Day_07___Camel_Cards
{
    public class CamelCards
    {
        public CamelCards(string[] input)
        {
            this.Hands = Parse(input);
            this.Cards = new() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
        }

        private Dictionary<string, int> Hands { get; }

        private List<char> Cards { get; }

        public long Play(bool wildcards = false)
        {
            this.OrderJoker(wildcards);
            return this.HandsToWinnings(this.HandsToStrengths(wildcards));
        }

        private static HandStrength HandToStrength(string hand)
        {
            Dictionary<char, int> cards = new();

            for(int i = 0; i < hand.Length; i++)
            {
                if (!cards.ContainsKey(hand[i]))
                {
                    cards.Add(hand[i], 1);
                }
                else
                {
                    cards[hand[i]]++;
                }
            }

            int max = cards.Max(x => x.Value);

            if (max == 5)
            {
                return HandStrength.FiveOfAKind;
            }
            else if (max == 4)
            {
                return HandStrength.FourOfAKind;
            }
            else if(max == 3)
            {
                return cards.Count == 2 ? HandStrength.FullHouse : HandStrength.ThreeOfAKind;
            }
            else if (cards.Count == 3 && max == 2 && cards.Min(x => x.Value) == 1)
            {
                return HandStrength.TwoPair;
            }

            return cards.Count == 5 ? HandStrength.HighCard : HandStrength.OnePair;
        }

        private static Dictionary<string, int> Parse(string[] input)
            => input.Select(x => x.Split(" ")).ToDictionary(x => x[0], x => int.Parse(x[1]));

        private Dictionary<string, HandStrength> HandsToStrengths(bool wildcards)
        {
            Dictionary<string, HandStrength> result = new();
            HashSet<HandStrength> hands = new();

            foreach (KeyValuePair<string, int> hand in this.Hands)
            {
                if (wildcards
                    && hand.Key.Contains('J'))
                {
                    hands.Clear();

                    foreach (char card in this.Cards.Where(x => x != 'J'))
                    {
                        hands.Add(HandToStrength(hand.Key.Replace('J', card)));
                    }

                    result.Add(hand.Key, hands.Min());
                }
                else
                {
                    result.Add(hand.Key, HandToStrength(hand.Key));
                }
            }

            return result;
        }

        private long HandsToWinnings(Dictionary<string, HandStrength> hands)
        {
            int rank = 1;
            long result = 0;

            foreach (KeyValuePair<string, HandStrength> hand in hands
                .OrderBy(x => x.Value)
                .ThenBy(x => this.OrderHand(x.Key))
                .Reverse())
            {
                result += this.Hands[hand.Key] * rank;
                rank++;
            }

            return result;
        }

        private void OrderJoker(bool wildcards)
        {
            if (wildcards)
            {
                this.Cards.Remove('J');
                this.Cards.Add('J');
            }
        }

        private string OrderHand(string value)
        {
            string result = string.Empty;

            foreach (char chr in value)
            {
                result += 'A' + this.Cards.IndexOf(chr);
            }

            return result;
        }
    }
}
