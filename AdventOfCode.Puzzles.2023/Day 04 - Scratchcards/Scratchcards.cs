namespace AdventOfCode.Puzzles._2023.Day_04___Scratchcards
{
    public class Scratchcards
    {
        public Scratchcards(string[] input)
        {
            this.Cards = new();

            for (int i = 0; i < input.Length; i++)
            {
                this.Cards.Add(i + 1, new(input[i]));
            }
        }

        private Dictionary<int, Card> Cards { get; }

        public int TotalPoints()
        {
            int result = 0;

            foreach (var card in this.Cards)
            {
                int points = 0;

                foreach (int number in card.Value.Numbers)
                {
                    if (card.Value.Winning.Contains(number))
                    {
                        points = points == 0 ? 1 : points * 2;
                    }
                }

                result += points;
            }

            return result;
        }

        public int TotalCards()
        {
            int result = 0;

            foreach (var card in this.Cards)
            {
                result += this.Scratch(card.Value);
            }

            return result;
        }

        private int Scratch(Card card)
        {
            int result = 0;
            int i = 0;
            result++;

            foreach (int number in card.Numbers)
            {
                if (card.Winning.Contains(number))
                {
                    result += this.Scratch(this.Cards[card.Number + ++i]);
                }
            }

            return result;
        }
    }
}
