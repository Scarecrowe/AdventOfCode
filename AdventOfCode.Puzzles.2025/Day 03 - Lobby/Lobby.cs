namespace AdventOfCode.Puzzles._2025.Day_03___Lobby
{
    public class Lobby(string[] input)
    {
        private string[] Batteries { get; set; } = input;

        public long Joltage(int count)
        {
            long result = 0;

            foreach (var line in this.Batteries)
            {
                int toRemove = line.Length - count;
                Stack<char> batteries = new(count);

                foreach (char battery in line)
                {
                    while (batteries.Count > 0 && toRemove > 0 && batteries.Peek() < battery)
                    {
                        batteries.Pop();
                        toRemove--;
                    }

                    batteries.Push(battery);
                }

                while (batteries.Count > count)
                {
                    batteries.Pop();
                }

                char[] picked = [.. batteries];
                Array.Reverse(picked);

                result += long.Parse(new string(picked));
            }

            return result;
        }
    }
}
