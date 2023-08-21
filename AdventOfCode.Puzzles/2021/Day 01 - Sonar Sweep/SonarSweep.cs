namespace AdventOfCode.Puzzles._2021.Day_01___Sonar_Sweep
{
    using AdventOfCode.Core.Extensions;

    public class SonarSweep
    {
        public static int LargerThan(string[] input)
        {
            int result = 0;
            int[] depths = input.ToInt();

            for (int i = 1; i < depths.Length; i++)
            {
                if (depths[i - 1] < depths[i])
                {
                    result++;
                }
            }

            return result;
        }

        public static string[] GroupAndSum(string[] input)
        {
            List<int> result = new();
            int[] depths = input.ToInt();

            for (int i = 0; i < depths.Length; i++)
            {
                int sum = 0;

                if (i == depths.Length - 2)
                {
                    break;
                }

                for (int j = 0; j < 3; j++)
                {
                    sum += depths[i + j];
                }

                result.Add(sum);
            }

            return result.Select(x => $"{x}").ToArray();
        }
    }
}
