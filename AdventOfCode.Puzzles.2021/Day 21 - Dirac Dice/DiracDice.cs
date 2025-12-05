namespace AdventOfCode.Puzzles._2021.Day_21___Dirac_Dice
{
    using AdventOfCode.Core.Extensions;

    public class DiracDice
    {
        private static readonly (int, int)[] RollFrequency = new(int, int)[] { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };

        public DiracDice(string[] input)
        {
            this.Players = new();
            this.Wins = new long[2];

            foreach (string line in input)
            {
                int[] tokens = line.Replace("Player ").Split(" starting position: ").ToInt();

                this.Players.Add(tokens[0], new(tokens[0], tokens[1]));
            }
        }

        public static Dictionary<int, int> PossibleRolls { get; } = CreateRolls();

        public static Dictionary<(int start, int roll), int> Lookup { get; } = CreateLookup();

        public Dictionary<int, Player> Players { get; }

        public long[] Wins { get; }

        private int DieCount { get; set; }

        private int DiceValue { get; set; }

        public static Dictionary<(int start, int roll), int> CreateLookup()
        {
            Dictionary<(int start, int roll), int> result = new();

            int[] rolls = new int[] { 3, 4, 5, 6, 7, 8, 9 };

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 0; j < rolls.Length; j++)
                {
                    int position = i;

                    for (int k = 1; k <= rolls[j]; k++)
                    {
                        position++;

                        if (position > 10)
                        {
                            position = 1;
                        }
                    }

                    result.Add((i, rolls[j]), position);
                }
            }

            return result;
        }

        public static Dictionary<int, int> CreateRolls()
        {
            List<int> rolls = new();

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        rolls.Add(i + j + k);
                    }
                }
            }

            Dictionary<int, int> rollAndCount = new();

            foreach (int roll in rolls)
            {
                if (!rollAndCount.ContainsKey(roll))
                {
                    rollAndCount.Add(roll, 0);
                }

                rollAndCount[roll]++;
            }

            return rollAndCount;
        }

        public int RollDice()
        {
            this.DieCount++;
            this.DiceValue++;

            if (this.DiceValue > 100)
            {
                this.DiceValue = 1;
            }

            return this.DiceValue;
        }

        public int RollDie()
        {
            int roll = 0;

            for (int i = 1; i <= 3; i++)
            {
                roll += this.RollDice();
            }

            return roll;
        }

        public long PlayQuantum()
        {
            var (p1, p2) = this.RecursiveQuantum(this.Players[1].Position - 1, 21, this.Players[2].Position - 1, 21);

            return p1 > p2 ? p1 : p2;
        }

        public long Play()
        {
            bool canPlay = true;
            Player player1 = this.Players.First().Value;
            Player player2 = this.Players.Last().Value;
            Player player = player1;

            while (canPlay)
            {
                player.IncrementPosition(this.RollDie());
                player.IncrementScore();

                if (player.Score >= 1000)
                {
                    canPlay = false;
                    break;
                }

                player = player == player1 ? player2 : player1;
            }

            long minScore = this.Players.Min(x => x.Value.Score);

            return minScore * this.DieCount;
        }

        private (long scoreA, long scoreB) RecursiveQuantum(long positionA, long t1, long positionB, long t2)
        {
            if (t2 <= 0)
            {
                return (0, 1);
            }

            long scoreA = 0;
            long scoreB = 0;

            foreach (var (role, frequency) in RollFrequency)
            {
                var (c2, c1) = this.RecursiveQuantum(positionB, t2, (positionA + role) % 10, t1 - 1 - ((positionA + role) % 10));
                scoreA += frequency * c1;
                scoreB += frequency * c2;
            }

            return (scoreA, scoreB);
        }
    }
}
