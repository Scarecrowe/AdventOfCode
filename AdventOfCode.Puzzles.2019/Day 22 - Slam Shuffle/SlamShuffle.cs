namespace AdventOfCode.Puzzles._2019.Day_22___Slam_Shuffle
{
    using System.Numerics;
    using AdventOfCode.Core.Extensions;

    public class SlamShuffle
    {
        public SlamShuffle(string[] input)
        {
            int deckSize = 10007;
            this.Input = input;
            this.Deck = new(deckSize);
            this.Deck.Fill(0, deckSize - 1);
        }

        private List<int> Deck { get; set; }

        private string[] Input { get; }

        public int Shuffle()
        {
            foreach (string line in this.Input)
            {
                if (line == "deal into new stack")
                {
                    this.ShuffleStack();
                }
                else if (line.StartsWith("cut"))
                {
                    this.ShuffleCut(line.Replace("cut ").ToInt());
                }
                else
                {
                    this.ShuffleIncrement(line.Replace("deal with increment ").ToInt());
                }
            }

            return this.Deck.IndexOf(2019);
        }

        public BigInteger ShuffleLargerDeck()
        {
            BigInteger size = 119315717514047;
            BigInteger iter = 101741582076661;
            BigInteger offset_diff = 0;
            BigInteger increment_mul = 1;

            foreach (string line in this.Input)
            {
                ShuffleLargerDeck(ref increment_mul, ref offset_diff, size, line);
            }

            (BigInteger increment, BigInteger offset) = GetSequence(iter, increment_mul, offset_diff, size);

            return Get(offset, increment, 2020, size);
        }

        private static (BigInteger increment, BigInteger offset) GetSequence(BigInteger iterations, BigInteger inc_mul, BigInteger offset_diff, BigInteger size)
        {
            var increment = inc_mul.ModPow(iterations, size);

            var offset = offset_diff * (1 - increment) * ((1 - inc_mul) % size).Inv(size);

            offset %= size;

            return (increment, offset);
        }

        private static BigInteger Get(BigInteger offset, BigInteger increment, BigInteger i, BigInteger size) => (offset + (i * increment)) % size;

        private static void ShuffleLargerDeck(ref BigInteger inc_mul, ref BigInteger offset_diff, BigInteger size, string line)
        {
            if (line.StartsWith("cut"))
            {
                offset_diff += line.Split(" ").Last().ToInt() * inc_mul;
            }
            else if (line == "deal into new stack")
            {
                inc_mul *= -1;
                offset_diff += inc_mul;
            }
            else
            {
                int num = line.Split(" ").Last().ToInt();
                inc_mul *= num.ToBigInteger().Inv(size);
            }

            inc_mul = inc_mul.Mod(size);
            offset_diff = offset_diff.Mod(size);
        }

        private void ShuffleCut(int index)
        {
            if (index > 0)
            {
                IEnumerable<int> cut = this.Deck.Take(index);
                this.Deck = this.Deck.Skip(index).ToList();
                this.Deck.AddRange(cut);
            }
            else
            {
                IEnumerable<int> cut = this.Deck.GetRange(this.Deck.Count - Math.Abs(index), Math.Abs(index));
                this.Deck = this.Deck.Take(this.Deck.Count - Math.Abs(index)).ToList();
                this.Deck.InsertRange(0, cut);
            }
        }

        private void ShuffleIncrement(int increment)
        {
            int[] deck = new int[this.Deck.Count];

            for (int i = 0; i < this.Deck.Count; i++)
            {
                deck[(increment * i) % this.Deck.Count] = this.Deck[i];
            }

            this.Deck = deck.ToList();
        }

        private void ShuffleStack() => this.Deck.Reverse();
    }
}
