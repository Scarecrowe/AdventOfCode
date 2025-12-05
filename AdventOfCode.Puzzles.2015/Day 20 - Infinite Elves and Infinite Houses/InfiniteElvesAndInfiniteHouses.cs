namespace AdventOfCode.Puzzles._2015.Day_20___Infinite_Elves_and_Infinite_Houses
{
    using AdventOfCode.Core.Extensions;

    public class InfiniteElvesAndInfiniteHouses
    {
        public InfiniteElvesAndInfiniteHouses(string[] input) => this.NumberOfHouses = input[0].ToInt();

        public int NumberOfHouses { get; }

        public int Lowest(bool infinite)
        {
            int min = 1;

            while (Sum(min, infinite) < this.NumberOfHouses)
            {
                min += 1;
            }

            return min;
        }

        private static int Sum(int value, bool limit)
        {
            int sum = 0;
            int root = (int)Math.Sqrt(value) + 1;

            for (int i = 1; i <= root; i++)
            {
                if (value % i == 0)
                {
                    if (!limit)
                    {
                        sum += i;
                        sum += value / i;

                        continue;
                    }

                    if (i <= 50)
                    {
                        sum += value / i;
                    }

                    if (value / i <= 50)
                    {
                        sum += i;
                    }
                }
            }

            return sum * (limit ? 11 : 10);
        }
    }
}
