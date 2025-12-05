namespace AdventOfCode.Puzzles._2021.Day_07___The_Treachery_of_Whales
{
    using AdventOfCode.Core.Extensions;

    public class TheTreacheryOfWhales
    {
        public TheTreacheryOfWhales(string[] input) => this.Input = input[0].Split(',').ToInt();

        public int[] Input { get; }

        public int Calculate(bool isConstant)
        {
            int leastFuel = int.MaxValue;

            for (int i = 0; i < this.Input.Max(); i++)
            {
                int fuel = 0;

                for (int j = 0; j < this.Input.Length; j++)
                {
                    int moves = Math.Abs(i - this.Input[j]);
                    fuel += isConstant ? moves : moves * (moves + 1) / 2;

                    if (fuel >= leastFuel)
                    {
                        break;
                    }
                }

                leastFuel = Math.Min(leastFuel, fuel);
            }

            return leastFuel;
        }
    }
}
