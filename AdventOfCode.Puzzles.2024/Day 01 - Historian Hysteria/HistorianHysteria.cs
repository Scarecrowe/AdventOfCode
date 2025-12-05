namespace AdventOfCode.Puzzles._2024.Day_01___Historian_Hysteria
{
    public class HistorianHysteria
    {
        public List<int> ListA { get; private set; }

        public List<int> ListB { get; private set; }

        public HistorianHysteria(string[] input)
        {
            this.ListA = new();
            this.ListB = new();
            this.Parse(input);
        }

        private void Parse(string[] input)
        {
            foreach (string line in input)
            {
                int[] values = line.Split("   ").Select(x => int.Parse(x)).ToArray();
                this.ListA.Add(values[0]);
                this.ListB.Add(values[1]);
            }

            this.ListA.Sort();
            this.ListB.Sort();
        }

        public int Distance()
        {
            int distance = 0;

            for (int i = 0; i < this.ListA.Count; i++)
            {
                distance += Math.Abs(this.ListA[i] - this.ListB[i]);
            }

            return distance;
        }

        public int Similarity()
        {
            int similarity = 0;

            Dictionary<int, int> frequency = new();

            foreach (int value in this.ListB)
            {
                frequency[value] = frequency.ContainsKey(value) ? frequency[value] + 1 : 1;
            }

            foreach (int value in this.ListA)
            {
                if (frequency.ContainsKey(value))
                {
                    similarity += value * frequency[value];
                }
            }

            return similarity;
        }
    }
}
