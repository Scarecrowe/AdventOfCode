namespace AdventOfCode.Puzzles._2024.Day_11___Plutonian_Pebbles
{
    public class PlutonianPebbles()
    {
        public Dictionary<PlutonianKey, long> Blinked { get; private set; } = [];

        public long Blink(string[] input, int count) => input[0].Split(' ').Select(x => int.Parse(x)).Sum(x => Depth(x, count));

        private long Depth(long stone, int count)
        {
            PlutonianKey key = new(stone, count);

            if (count == 0)
            {
                return 1;
            }
            else if (this.Blinked.TryGetValue(key, out var blinked))
            {
                return blinked;
            }

            long result = Rule(stone, count);

            this.Blinked[key] = result;

            return result;
        }

        private long Rule(long stone, int count)
        {
            long result;

            if (stone == 0)
            {
                result = Depth(1, count - 1);
            }
            else
            {
                int digits = (int)Math.Log10(stone) + 1;

                if (digits % 2 == 0)
                {
                    long divisor = (long)Math.Pow(10, digits / 2);
                    long left = stone / divisor;
                    long right = stone % divisor;

                    result = Depth(left, count - 1) + Depth(right == 0 ? 0 : right, count - 1);
                }
                else
                {
                    result = Depth(stone * 2024, count - 1);
                }
            }

            return result;
        }
    }
}
