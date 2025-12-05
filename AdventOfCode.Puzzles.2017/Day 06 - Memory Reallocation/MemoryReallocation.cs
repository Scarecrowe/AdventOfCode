namespace AdventOfCode.Puzzles._2017.Day_06___Memory_Reallocation
{
    using AdventOfCode.Core.Extensions;

    public class MemoryReallocation
    {
        public MemoryReallocation(string[] input)
        {
            this.Cycles = new();
            this.Banks = input[0].Split('\t').ToIntList();
        }

        public List<int> Banks { get; }

        public HashSet<string> Cycles { get; }

        public int Redistribute()
        {
            while (true)
            {
                string cycle = this.Banks.Join("\t");

                if (this.Cycles.Contains(cycle))
                {
                    return this.Cycles.Count;
                }

                this.Cycles.Add(cycle);

                int max = this.Banks.Max();
                int index = this.Banks.IndexOf(max);
                this.Banks[index] = 0;

                while (true)
                {
                    index = (index + 1) % this.Banks.Count;

                    this.Banks[index]++;
                    max--;

                    if (max == 0)
                    {
                        break;
                    }
                }
            }
        }

        public int InifititeCycles()
        {
            this.Redistribute();
            this.Cycles.Clear();
            return this.Redistribute();
        }
    }
}
