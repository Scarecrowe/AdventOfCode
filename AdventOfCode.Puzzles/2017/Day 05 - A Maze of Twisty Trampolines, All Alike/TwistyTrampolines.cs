namespace AdventOfCode.Puzzles._2017.Day_05___A_Maze_of_Twisty_Trampolines__All_Alike
{
    using AdventOfCode.Core.Extensions;

    public class TwistyTrampolines
    {
        public static int Simple(string[] input)
        {
            List<int> instructions = input.ToIntList();
            int index = 0;
            int steps = 0;

            while (true)
            {
                steps++;
                instructions[index]++;
                index += instructions[index] - 1;

                if (index < 0 || index >= instructions.Count)
                {
                    break;
                }
            }

            return steps;
        }

        public static int Advanced(string[] input)
        {
            List<int> instructions = input.ToIntList();
            int index = 0;
            int steps = 0;

            while (true)
            {
                steps++;

                int lastIndex = index;
                index += instructions[index];

                if (instructions[lastIndex] >= 3)
                {
                    instructions[lastIndex]--;
                }
                else
                {
                    instructions[lastIndex]++;
                }

                if (index < 0 || index >= instructions.Count)
                {
                    break;
                }
            }

            return steps;
        }
    }
}
