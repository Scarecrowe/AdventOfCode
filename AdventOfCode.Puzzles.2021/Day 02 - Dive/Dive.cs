namespace AdventOfCode.Puzzles._2021.Day_02___Dive
{
    using AdventOfCode.Core.Extensions;

    public class Dive
    {
        public static int Simple(string[] input)
        {
            int horizontal = 0;
            int depth = 0;

            foreach (string line in input)
            {
                string[] tokens = line.Split(' ');
                int value = tokens[1].ToInt();

                switch (tokens[0])
                {
                    case "forward":
                        horizontal += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                    case "down":
                        depth += value;
                        break;
                }
            }

            return horizontal * depth;
        }

        public static int Advanced(string[] input)
        {
            int horizontal = 0;
            int depth = 0;
            int aim = 0;

            foreach (string line in input)
            {
                string[] tokens = line.Split(' ');
                int value = tokens[1].ToInt();

                switch (tokens[0])
                {
                    case "forward":
                        horizontal += value;
                        depth += aim * value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }

            return horizontal * depth;
        }
    }
}
