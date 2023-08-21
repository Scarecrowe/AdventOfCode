namespace AdventOfCode.Puzzles._2018.Day_09___Marble_Mania
{
    using AdventOfCode.Core.Extensions;

    public class MarbleMania
    {
        public MarbleMania(string line)
        {
            string[] tokens = line.Split(" players; last marble is worth ");

            this.PlayerCount = tokens[0].ToInt();
            this.MarbleCount = tokens[1].Replace(" points").ToInt();
            this.Players = new long[this.PlayerCount];
        }

        public int PlayerCount { get; }

        public int MarbleCount { get; private set; }

        public long[] Players { get; }

        public long Play(int multiplier = 1)
        {
            this.MarbleCount *= multiplier;
            LinkedList<int> used = new();
            LinkedListNode<int> current = used.AddFirst(0);

            for (int i = 0; i < this.MarbleCount; i++)
            {
                if (((i + 1) % 23) == 0)
                {
                    for (int j = 1; j <= 7; j++)
                    {
                        current = Previous(ref current, ref used);
                    }

                    this.Players[i % this.PlayerCount] += i + 1 + current.Value;

                    LinkedListNode<int> previous = current;
                    current = Next(ref current, ref used);
                    used.Remove(previous);
                }
                else
                {
                    current = used.AddAfter(Next(ref current, ref used), i + 1);
                }
            }

            return this.Players.Max();
        }

        private static LinkedListNode<int> Next(ref LinkedListNode<int> current, ref LinkedList<int> used) => current.Next ?? used.First ?? new(0);

        private static LinkedListNode<int> Previous(ref LinkedListNode<int> current, ref LinkedList<int> used) => current.Previous ?? used.Last ?? new(0);
    }
}
