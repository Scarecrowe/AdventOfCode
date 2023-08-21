namespace AdventOfCode.Puzzles._2021.Day_21___Dirac_Dice
{
    public class Player
    {
        public Player(int number, int position)
        {
            this.Number = number;
            this.Position = position;
        }

        public Player(Player player)
        {
            this.Number = player.Number;
            this.Position = player.Position;
            this.Score = player.Score;
        }

        public int Number { get; }

        public int Position { get; private set; }

        public long Score { get; private set; }

        public void IncrementPosition(int roll)
        {
            for (int i = 1; i <= roll; i++)
            {
                this.Position++;

                if (this.Position > 10)
                {
                    this.Position = 1;
                }
            }
        }

        public void IncrementScore() => this.Score += this.Position;

        public List<(int roll, int score)> Scores(Dictionary<int, int> rolls)
        {
            List<(int roll, int score)> result = new();

            foreach (KeyValuePair<int, int> roll in rolls)
            {
                result.Add((roll.Key, (int)this.Score + DiracDice.Lookup[(this.Position, roll.Key)]));
            }

            return result;
        }
    }
}
