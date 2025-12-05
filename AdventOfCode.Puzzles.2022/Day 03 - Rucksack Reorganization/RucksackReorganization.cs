namespace AdventOfCode.Puzzles._2022.Day_03___Rucksack_Reorganization
{
    public class RucksackReorganization
    {
        public static int Sum(string[] input)
        {
            int sum = 0;

            foreach (string line in input)
            {
                foreach (char current in line[.. (line.Length / 2)])
                {
                    if (line[(line.Length / 2) ..].IndexOf(current) > -1)
                    {
                        sum += current;
                        sum -= char.IsLower(current) ? 96 : 38;
                        break;
                    }
                }
            }

            return sum;
        }

        public static int GroupSum(string[] input)
        {
            int sum = 0;

            for (int i = 0; i < input.Length; i += 3)
            {
                foreach (var current in input[i].GroupBy(x => x))
                {
                    if (input[i + 1].IndexOf(current.Key) > -1 && input[i + 2].IndexOf(current.Key) > -1)
                    {
                        sum += current.Key;
                        sum -= char.IsLower(current.Key) ? 96 : 38;
                        break;
                    }
                }
            }

            return sum;
        }
    }
}
