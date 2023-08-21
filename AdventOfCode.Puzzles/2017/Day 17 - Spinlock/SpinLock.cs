namespace AdventOfCode.Puzzles._2017.Day_17___Spinlock
{
    public class SpinLock
    {
        public static int Run(int cycles, int steps, int valueAfter)
        {
            List<int> values = new() { 0 };

            int position = 0;

            for (int i = 1; i <= cycles; i++)
            {
                int index = ((position + steps) % values.Count) + 1;
                values.Insert(index, i);
                position = index;
            }

            return values[values.IndexOf(valueAfter) + 1];
        }

        public static int RunAngry(int cycles, int steps)
        {
            int current = 1;
            int position = 0;
            int result = 0;

            for (int i = 0; i <= cycles; i++)
            {
                position = ((position + steps) % current) + 1;

                if (position == 1)
                {
                    result = i + 1;
                }

                current += 1;
            }

            return result;
        }
    }
}
