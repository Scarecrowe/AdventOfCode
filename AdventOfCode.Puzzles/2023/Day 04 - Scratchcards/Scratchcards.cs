namespace AdventOfCode.Puzzles._2023.Day_04___Scratchcards
{
    public class Scratchcards
    {
        public Scratchcards(string[] input)
        {
            this.Cards = new();
            this.Points = new();

            for (int i = 0; i < input.Length; i++)
            {
                this.Cards.Add(i + 1, new Card(input[i]));
            }
        }

        private Dictionary<int, Card> Cards { get; }

        private Dictionary<int, int> Points { get; }

        public int TotalPoints()
        {
            int result = 0;

            foreach (var card in this.Cards)
            {
                int points = 0;
                this.Points.Clear();

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
            foreach (var card in this.Cards)
            {
                this.Scratch(card.Value);
            }

            return this.Points.Sum(x => x.Value);
        }

        private void Scratch(Card card)
        {
            if (!this.Points.ContainsKey(card.Number))
            {
                this.Points.Add(card.Number, 1);
            }
            else
            {
                this.Points[card.Number]++;
            }

            int i = 0;

            foreach (int number in card.Numbers)
            {
                if (card.Winning.Contains(number))
                {
                    i += 1;
                    this.Scratch(this.Cards[card.Number + i]);
                }
            }
        }
    }
}
