namespace AdventOfCode.Puzzles._2020.Day_22___Crab_Combat
{
    using AdventOfCode.Core;

    public class Player
    {
        public Player(int id)
        {
            this.Id = id;
            this.Cards = new();
        }

        public Player(Player player, int card)
        {
            this.Id = player.Id;
            this.Cards = new();

            for (int i = 0; i < card; i++)
            {
                this.Cards.Add(player.Cards[i]);
            }
        }

        public int Id { get; }

        public List<int> Cards { get; private set; }

        public bool HasCards => this.Cards.Count > 0;

        public void AddCard(int card)
        {
            this.Cards.Add(card);
        }

        public int DrawCard()
        {
            int card = this.Cards[0];
            this.Cards.RemoveAt(0);

            return card;
        }

        public long Score()
        {
            long score = 0;

            for (int i = this.Cards.Count - 1; i >= 0; i--)
            {
                score += this.Cards[i] * (this.Cards.Count - i);
            }

            return score;
        }
    }
}
