namespace AdventOfCode.Puzzles._2016.Day_03___Squares_With_Three_Sides
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SquaresWithThreeSides
    {
        public static int Valid(string[] input)
        {
            int valid = 0;

            foreach (string line in input)
            {
                int[] sides = line.SplitSpace().ToInt();

                valid += MathHelper.IsTriangle(sides[0], sides[1], sides[2]) ? 1 : 0;
            }

            return valid;
        }

        public static int ValidByColumns(string[] input)
        {
            int valid = 0;

            int[,] numbers = new int[input.Length, 3];

            for (int y = 0; y < input.Length; y++)
            {
                int[] sides = input[y].SplitSpace().ToInt();

                for (int x = 0; x < sides.Length; x++)
                {
                    numbers[y, x] = sides[x];
                }
            }

            for (int x = 0; x <= 2; x++)
            {
                for (int y = 0; y < input.Length - 2; y += 3)
                {
                    valid += MathHelper.IsTriangle(numbers[y, x], numbers[y + 1, x], numbers[y + 2, x]) ? 1 : 0;
                }
            }

            return valid;
        }
    }
}
