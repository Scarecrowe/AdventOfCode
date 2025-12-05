namespace AdventOfCode.Puzzles._2021.Day_21___Dirac_Dice
{
    public class Universe
    {
        public Universe(Player playerA, Player playerB)
        {
            this.PlayerA = new(playerA);
            this.PlayerB = new(playerB);
        }

        public Universe(Universe universe)
        {
            this.PlayerA = new(universe.PlayerA);
            this.PlayerB = new(universe.PlayerB);
        }

        public Player PlayerA { get; }

        public Player PlayerB { get; }

        public Player? Play((int a, int b) roll)
        {
            this.PlayerA.IncrementPosition(roll.a);
            this.PlayerA.IncrementScore();

            if (this.PlayerA.Score >= 21)
            {
                return this.PlayerA;
            }

            this.PlayerB.IncrementPosition(roll.b);
            this.PlayerB.IncrementScore();

            if (this.PlayerB.Score >= 21)
            {
                return this.PlayerB;
            }

            return null;
        }
    }
}
