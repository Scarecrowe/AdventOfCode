namespace AdventOfCode.Puzzles._2020.Day_22___Crab_Combat
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class CrabCombat
    {
        public CrabCombat(string[] input)
        {
            this.Players = new();

            Player? player = null;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line) && player != null)
                {
                    this.Players.Add(player);
                    continue;
                }

                if (line.StartsWith("P"))
                {
                    player = new(line.Replace("Player ").Replace(":").ToInt());
                    continue;
                }

                player?.AddCard(line.ToInt());
            }

            this.Players.Add(player ?? new(0));
        }

        public List<Player> Players { get; private set; }

        public Player Winner => this.Players.First(x => x.Cards.Count > 0);

        public long Play()
        {
            Dictionary<int, int> cards = new();

            int round = 1;
            bool finished = false;

            while (!finished)
            {
                cards = new();

                foreach (Player player in this.Players)
                {
                    int card = player.DrawCard();
                    cards.Add(player.Id, card);
                }

                Player winner = this.Players.First(player => player.Id == cards.First(x => x.Value == cards.Values.Max()).Key);

                if (winner.Id == 2)
                {
                    cards = cards.Reverse().ToDictionary(x => x.Key, x => x.Value);
                }

                foreach (KeyValuePair<int, int> card in cards)
                {
                    winner.AddCard(card.Value);
                }

                finished = this.Players.Count(x => x.Cards.Count > 0) == 1;

                round++;
            }

            return this.Winner.Score();
        }

        public Player PlayRecursive(List<Player> players, int game)
        {
            Dictionary<int, List<List<int>>> decks = new();
            Dictionary<int, int>? cards = null;
            Player? winner = null;

            int round = 1;
            bool finished = false;

            while (!finished)
            {
                cards = new();

                foreach (Player player in players)
                {
                    if (decks.ContainsKey(player.Id))
                    {
                        foreach (List<int> deck in decks[player.Id])
                        {
                            if (player.Cards.SequenceEqual(deck))
                            {
                                return players[0];
                            }
                        }
                    }

                    if (!decks.ContainsKey(player.Id))
                    {
                        decks.Add(player.Id, new());
                    }

                    decks[player.Id].Add(new(player.Cards));

                    int card = player.DrawCard();

                    cards.Add(player.Id, card);
                }

                winner = null;

                if (players[0].Cards.Count >= cards[1] && players[1].Cards.Count >= cards[2])
                {
                    List<Player> subGamePlayers = new()
                    {
                        new(players[0], cards[1]),
                        new(players[1], cards[2])
                    };

                    winner = this.PlayRecursive(subGamePlayers, game + 1);
                }

                if (winner == null)
                {
                    winner = players.FirstOrDefault(player => player.Id == cards.FirstOrDefault(x => x.Value == cards.Values.Max()).Key);
                }

                if (winner?.Id == 2)
                {
                    cards = cards.Reverse().ToDictionary(x => x.Key, x => x.Value);
                }

                foreach (KeyValuePair<int, int> card in cards)
                {
                    players[(winner?.Id ?? 0) - 1].AddCard(card.Value);
                }

                finished = players.Count(x => x.Cards.Count > 0) == 1;

                round++;
            }

            return winner ?? new(0);
        }
    }
}
